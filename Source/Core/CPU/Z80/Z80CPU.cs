using System;
using System.Collections.Generic;
using System.Threading;
using Morphic.Core.Bus;
using Morphic.Core.Primitive;

namespace Morphic.Core.CPU.Z80
{
    public partial class Z80CPU
    {
        public enum IndexerRegister
        {
            HL,
            IX,
            IY
        }

        public enum InstructionSets
        {
            Normal,
            CB,
            ED
        }

        public enum InterruptModes
        {
            IM0,
            IM1,
            IM2
        }

        public enum MemoryMode
        {
            Read,
            Write
        };

        public enum MemorySize
        {
            Byte = 8,
            Word = 16
        };


        public delegate void Opcall();

        public class Op
        {
            public Op(string name)
            {
                Name = name;
                call = delegate { throw new NotImplementedException(name); };
            }

            public Op(string name, Opcall call)
                : this(name)
            {
                this.call = call;
            }

            public string Name { get; }
            readonly Opcall call;

            public void Call()
            {
                call();
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public class StateOp : Op
        {
            public StateOp(string name, Opcall call)
                : base(name, call)
            {
            }
        }

        // Opcode tables

        // General registers
        public LEWord AF;
        public LEWord BC;
        public Dictionary<UInt16, Delegate> Breakpoints = new Dictionary<UInt16, Delegate>();
        public LEWord DE;
        public Byte I;
        public Boolean IFF1, IFF2;

        // Index registers
        public LEWord IX;
        public LEWord IY;
        public IBus16Bit Memory;

        // Internal registers
        public LEWord PC;
        public Byte R;
        public LEWord SP;
        public Boolean Step = false;
        public UInt16? StopAfterAddress;
        public UInt16? StopBeforeAddress;
        public LEWord altAF;
        public LEWord altBC;
        public LEWord altDE;
        public LEWord altHL;
        protected IndexerRegister indexerRegister;
        protected InstructionSets instructionSet;

        // Processor state
        protected InterruptModes interruptMode;
        public IBus16Bit io;
        public LastMemory lastMemory;
        public Op[] op = new Op[256]; // Primary instruction set
        public Op[] opCB = new Op[256]; // CB instruction set
        public Op[] opED = new Op[256]; // ED instruction set
        public LEWord rHL; // REAL HL
        public UInt16 tStates;
        public Byte x8;

        public InterruptModes InterruptMode => interruptMode;

        public InstructionSets InstructionSet => instructionSet;

        public IndexerRegister IndexerRegisterMode => indexerRegister;

        public struct LastMemory
        {
            public ushort Address;
            public MemoryMode Mode;
            public MemorySize Size;
        }

        SByte? indexerOffset;

        public UInt16 ProgramCounter => (InstructionSet == InstructionSets.Normal) ? PC.ToUInt16() : (UInt16)(PC.ToUInt16() - 1);

        public Byte A
        {
            get { return AF.High; }
            set { AF.High = value; }
        }

        public Byte F
        {
            get { return AF.Low; }
            set { AF.Low = value; }
        }

        public Byte B
        {
            get { return BC.High; }
            set { BC.High = value; }
        }

        public Byte C
        {
            get { return BC.Low; }
            set { BC.Low = value; }
        }

        public Byte D
        {
            get { return DE.High; }
            set { DE.High = value; }
        }

        public Byte E
        {
            get { return DE.Low; }
            set { DE.Low = value; }
        }

        public Byte H
        {
            get
            {
                switch (indexerRegister)
                {
                    case IndexerRegister.IX:
                        return IX.High;
                    case IndexerRegister.IY:
                        return IY.High;
                    default:
                        return rHL.High;
                }
            }
            set
            {
                switch (indexerRegister)
                {
                    case IndexerRegister.IX:
                        IX.High = value;
                        break;
                    case IndexerRegister.IY:
                        IY.High = value;
                        break;
                    default:
                        rHL.High = value;
                        break;
                }
            }
        }

        public Byte L
        {
            get
            {
                switch (indexerRegister)
                {
                    case IndexerRegister.IX:
                        return IX.Low;
                    case IndexerRegister.IY:
                        return IY.Low;
                    default:
                        return rHL.Low;
                }
            }
            set
            {
                switch (indexerRegister)
                {
                    case IndexerRegister.IX:
                        IX.Low = value;
                        break;
                    case IndexerRegister.IY:
                        IY.Low = value;
                        break;
                    default:
                        rHL.Low = value;
                        break;
                }
            }
        }

        // Indirect access to HL with necessary offset applied
        public Byte HLAddr
        {
            get { return Mem8(HLe); }
            set
            {
                if (indexerRegister != IndexerRegister.HL)
                {
                    indexerOffset = (sbyte)value;
                    value = n;
                }

                Mem(HLe, value);
            }
        }

        // Fake HL taking current IX/IY into account with next byte 3 applied
        public LEWord HLe
        {
            get
            {
                if (indexerRegister == IndexerRegister.HL)
                    return rHL;

                if (!indexerOffset.HasValue)
                    indexerOffset = GetE();

                switch (indexerRegister)
                {
                    case IndexerRegister.IX:
                        var addressIX = IX.ToUInt16() + indexerOffset.Value;
                        return new LEWord((UInt16)addressIX);
                    case IndexerRegister.IY:
                        var addressIY = IY.ToUInt16() + indexerOffset.Value;
                        return new LEWord((UInt16)addressIY);
                }

                throw new NotSupportedException();
            }
        }

        // Fake HL taking current IX/IY into account
        public LEWord HL
        {
            get
            {
                switch (indexerRegister)
                {
                    case IndexerRegister.IX:
                        return IX;
                    case IndexerRegister.IY:
                        return IY;
                    default:
                        return rHL;
                }
            }
            set
            {
                switch (indexerRegister)
                {
                    case IndexerRegister.IX:
                        IX = value;
                        break;
                    case IndexerRegister.IY:
                        IY = value;
                        break;
                    default:
                        rHL = value;
                        break;
                }
            }
        }

        public string Flags
        {
            get
            {
                var flags = new string[8];
                flags[0] = (F & FlagMask.C) != 0 ? "C" : "c";
                flags[1] = (F & FlagMask.N) != 0 ? "N" : "n";
                flags[2] = (F & FlagMask.V) != 0 ? "V" : "v";
                flags[3] = (F & FlagMask.B3) != 0 ? "3" : "-";
                flags[4] = (F & FlagMask.H) != 0 ? "H" : "h";
                flags[5] = (F & FlagMask.B5) != 0 ? "5" : "-";
                flags[6] = (F & FlagMask.Z) != 0 ? "Z" : "z";
                flags[7] = (F & FlagMask.S) != 0 ? "S" : "s";
                return String.Join("", flags);
            }
        }

        // Register flag checking
        public bool TestC => (F & FlagMask.C) != 0;

        public bool TestNC => !TestC;

        public bool TestZ => (F & FlagMask.Z) != 0;

        public bool TestNZ => !TestZ;

        public bool TestPE => (F & FlagMask.P) != 0;

        public bool TestPO => !TestPE;

        public bool TestP => (F & FlagMask.S) != 0;

        public bool TestM => !TestP;

        public Z80CPU(IBus16Bit memory, IBus16Bit io)
        {
            Memory = memory;
            this.io = io;

            InitializeManualOpcodeTables();
            InitializeGeneratedOpcodeTables();
            InitializeFlagTables();
            Reset();
        }

        private void SetFlags(Byte flags)
        {
            F |= flags;
        }

        private void ClearFlags(Byte flags)
        {
            F = (Byte)(F & ~flags);
        }

        private void InitializeManualOpcodeTables()
        {
            // IX and IY prefixing
            op[0xDD] = new StateOp("IX", delegate { indexerRegister = IndexerRegister.IX; });
            op[0xFD] = new StateOp("IY", delegate { indexerRegister = IndexerRegister.IY; });

            // Load 8-bit accumulator <- register-address or value-address
            op[0x0A] = new Op("LD A,(BC)", delegate { A = Mem8(BC); });
            op[0x1A] = new Op("LD A,(DE)", delegate { A = Mem8(DE); });
            op[0x3A] = new Op("LD A,(nn)", delegate { A = Mem8(nn); });

            // Load 8-bit register-address or value-address <- accumulator
            op[0x02] = new Op("LD (BC),A", delegate { Mem(BC, A); });
            op[0x12] = new Op("LD (DE),A", delegate { Mem(DE, A); });
            op[0x32] = new Op("LD (nn),A", delegate { Mem(nn, A); });

            op[0x36] = new Op("LD (HL),n", delegate
            {
                if (indexerRegister != IndexerRegister.HL)
                {
                    indexerOffset = (sbyte)Mem8(PC);
                    PC.Inc();
                    HLAddr = Mem8(PC);
                }

                HLAddr = n;
            });

            // Load 8-bit accumulator <-> special register
            opED[0x47] = new Op("LD I,A", delegate
            {
                I = A;
                tStates += 1;
            });
            opED[0x57] = new Op("LD A,I", delegate
            {
                A = I;
                tStates += 1;
            });
            opED[0x4F] = new Op("LD R,A", delegate
            {
                R = A;
                tStates += 1;
            });
            opED[0x5F] = new Op("LD A,R", delegate
            {
                A = R;
                tStates += 1;
            });

            // n instructions
            op[0xC6] = new Op("ADD A,n", delegate { ADD(n); });
            op[0xCE] = new Op("ADC A,n", delegate { ADC(n); });
            op[0xD6] = new Op("SUB A,n", delegate { SUB(n); });
            op[0xDE] = new Op("SBC A,n", delegate { SBC(n); });
            op[0xE6] = new Op("AND n", delegate { AND(n); });
            op[0xEE] = new Op("XOR n", delegate { XOR(n); });
            op[0xF6] = new Op("OR n", delegate { OR(n); });
            op[0xFE] = new Op("CP n", delegate { CP(n); });

            // Exchange
            op[0xEB] = new Op("EX DE,HL", delegate
            {
                var t = DE;
                DE = rHL;
                rHL = t;
            });
            op[0x08] = new Op("EX AF,AF'", delegate { EX(ref AF, ref altAF); });
            op[0xD9] = new Op("EXX", delegate
            {
                EX(ref BC, ref altBC);
                EX(ref DE, ref altDE);
                EX(ref rHL, ref altHL);
            });
            op[0xE3] = new Op("EX (SP),HL", delegate
            {
                var t = rHL;
                rHL = Mem16(SP);
                Mem(SP, t);
            });

            // Block transfer
            opED[0xA0] = new Op("LDI", delegate
            {
                LDx();
                rHL = INC(rHL);
                DE = INC(DE);
            });
            opED[0xA8] = new Op("LDD", delegate
            {
                LDx();
                rHL = DEC(rHL);
                DE = DEC(DE);
            });
            opED[0xB0] = new Op("LDIR", delegate
            {
                LDx();
                rHL = INC(rHL);
                DE = INC(DE);
                if (BC != 0)
                {
                    PC.Dec();
                    PC.Dec();
                }
            });
            opED[0xB8] = new Op("LDDR", delegate
            {
                LDx();
                rHL = DEC(rHL);
                DE = DEC(DE);
                if (BC != 0)
                {
                    PC.Dec();
                    PC.Dec();
                }
            });

            // Compare
            opED[0xA1] = new Op("CPI", delegate
            {
                CPx();
                rHL = INC(rHL);
            });
            opED[0xA9] = new Op("CPD", delegate
            {
                CPx();
                rHL = DEC(rHL);
            });
            opED[0xB1] = new Op("CPIR", delegate
            {
                CPx();
                rHL = INC(rHL);
                if (BC != 0)
                {
                    PC.Dec();
                    PC.Dec();
                }
            });
            opED[0xB9] = new Op("CPDR", delegate
            {
                CPx();
                rHL = DEC(rHL);
                if (BC != 0)
                {
                    PC.Dec();
                    PC.Dec();
                }
            });

            // CPU control
            op[0x00] = new Op("NOP", delegate { });
            op[0x76] = new Op("HALT");
            op[0x27] = new Op("DAA");
            op[0x2F] = new Op("CPL", delegate { A ^= Byte.MaxValue; });
            op[0x37] = new Op("SCF", delegate { F |= FlagMask.C; });
            op[0x3F] = new Op("CCF", delegate { CCF(); });
            opED[0x44] = new Op("NEG", delegate { A = (Byte)(0 - A); });
            op[0xF3] = new Op("DI", delegate { IFF1 = IFF2 = false; });
            op[0xFB] = new Op("EI", delegate { IFF1 = IFF2 = true; });
            opED[0x46] = new Op("IM 0", delegate { interruptMode = InterruptModes.IM0; });
            opED[0x56] = new Op("IM 1", delegate { interruptMode = InterruptModes.IM1; });
            opED[0x5E] = new Op("IM 2", delegate
            {
                tStates += 4;
                interruptMode = InterruptModes.IM2;
            });
            op[0xCB] = new StateOp("CB set", delegate { instructionSet = InstructionSets.CB; });
            op[0xED] = new StateOp("ED set", delegate { instructionSet = InstructionSets.ED; });

            // Rotate & shift
            op[0x07] = new Op("RLCA");
            op[0x0F] = new Op("RRCA", delegate
                {
                    var carry = (A & 0x1) != 0;
                    A = (Byte)(A >> 1);
                    if (carry)
                    {
                        SetFlags(FlagMask.C);
                        F |= FlagMask.C;
                        A = (Byte)(A | 0x80);
                    }
                    else
                        ClearFlags(FlagMask.C);

                    ClearFlags(FlagMask.H);
                    ClearFlags(FlagMask.N);
                });
            op[0x17] = new Op("RLA");
            op[0x1F] = new Op("RRA", delegate
            {
                var carry = (F & FlagMask.C) != 0;
                A = (Byte)(A >> 1);
                if ((A & 0xF0) != 0)
                    SetFlags(FlagMask.C);
                else
                    ClearFlags(FlagMask.C);
                if (carry)
                    A |= (Byte)(A & 0xF0);
                else
                    A = (Byte)(A & ~0xF0);

                ClearFlags(FlagMask.H);
                ClearFlags(FlagMask.N);
            });
            op[0x67] = new Op("RRD");
            op[0x6F] = new Op("RLD");

            // Input & output
            op[0xD3] = new Op("OUT (n),A", delegate { OUT(n, A); });
            op[0xDB] = new Op("IN A,(n)", delegate { A = io.ReadByte(n); });
            opED[0xA2] = new Op("INI");
            opED[0xA3] = new Op("OUTI");
            opED[0xAA] = new Op("IND");
            opED[0xAB] = new Op("OUTD");
            opED[0xB2] = new Op("INIR");
            opED[0xB3] = new Op("OTIR");
            opED[0xBA] = new Op("INDR");
            opED[0xBB] = new Op("OTDR");

            // Special loads 16-bit
            op[0x22] = new Op("LD (nn),HL", delegate { Mem(nn, rHL); });
            op[0x2A] = new Op("LD HL,(nn)", delegate { rHL = Mem16(nn); });
            op[0xF9] = new Op("LD SP,HL", delegate { SP = rHL; });

            // Jump
            op[0x10] = new Op("DJNZ e", delegate
            {
                B = DEC(B);
                JR(B != 0, GetE());
            });
            op[0x18] = new Op("JR e", delegate { JR(true, GetE()); });
            op[0xC3] = new Op("JP nn", delegate { JP(true, nn); });
            op[0xE9] = new Op("JP (HL)", delegate { JP(true, rHL); });

            // Call & return
            op[0xC9] = new Op("RET", delegate { RET(true); });
            opED[0x45] = new Op("RETN");
            opED[0x4D] = new Op("RETI");
            op[0xCD] = new Op("CALL nn", delegate { CALL(true, nn); });
        }

        public void Cycle()
        {
            var opcode = Memory.ReadByte(PC);

            var address = PC.ToUInt16();
            var shouldStop = (StopAfterAddress != null && StopAfterAddress == address);

            if (address == StopBeforeAddress || Breakpoints.ContainsKey(address) ||
                (Step && instructionSet == InstructionSets.Normal))
            {
                if (address == StopBeforeAddress)
                    StopBeforeAddress = null;
                Thread.CurrentThread.Suspend();
            }

            // R is a 7-bit counter with bit 8 preserved
            var p = (Byte)((R & 127) + 1); // Increment 7-bit version
            R &= 128; // Clear everything but bit 8
            R |= (Byte)(p & 127); // Join them back together

            IncPC(1);
            Op cycleOp = null;

            switch (instructionSet)
            {
                case InstructionSets.Normal:
                    cycleOp = op[opcode];
                    break;
                case InstructionSets.CB:
                    if (indexerRegister != IndexerRegister.HL)
                    {
                        indexerOffset = (SByte)opcode;
                        opcode = n;
                    }
                    cycleOp = (opCB[opcode] ?? op[opcode]);
                    break;
                case InstructionSets.ED:
                    cycleOp = (opED[opcode] ?? op[opcode]);
                    break;
            }

            cycleOp.Call();

            // Reset after normal instructions?  Figure out when Z80 does this.
            if (!(cycleOp is StateOp))
            {
                instructionSet = InstructionSets.Normal;
                indexerRegister = IndexerRegister.HL;
                indexerOffset = null;
            }
            else
            {
                if (shouldStop)
                    StopAfterAddress = null;
                //Thread.CurrentThread.Suspend();
            }

            tStates += 4;
        }

        public void Reset()
        {
            indexerRegister = IndexerRegister.HL;
            instructionSet = InstructionSets.Normal;

            AF = new LEWord();
            BC = new LEWord();
            DE = new LEWord();
            rHL = new LEWord();
            altAF = new LEWord();
            altBC = new LEWord();
            altDE = new LEWord();
            altHL = new LEWord();

            I = 0;
            R = 0;
            SP = 0x0000;
            PC = 0x0000;
            tStates = 0;

            interruptMode = InterruptModes.IM0;
            IFF1 = IFF2 = false;
        }

        private UInt16 lastNat;

        protected Byte n
        {
            get
            {
                var next = Memory.ReadByte(PC);
                if (lastNat != PC)
                    IncPC(1);
                lastNat = PC;
                tStates += 3;
                return next;
            }
        }

        protected LEWord nn
        {
            get
            {
                LEWord next = Memory.ReadWord(PC);
                IncPC(2);
                tStates += 6;
                return next;
            }
        }

        protected SByte GetE()
        {
            return (SByte)n;
        }

        protected LEWord Mem16(LEWord address)
        {
            tStates += 3;
            lastMemory.Address = address;
            lastMemory.Size = MemorySize.Word;
            lastMemory.Mode = MemoryMode.Read;
            return Memory.ReadWord(address.ToUInt16());
        }

        protected Byte Mem8(LEWord address)
        {
            tStates += 3;
            lastMemory.Address = address;
            lastMemory.Size = MemorySize.Byte;
            lastMemory.Mode = MemoryMode.Read;
            return Memory.ReadByte(address.ToUInt16());
        }

        protected void Mem(LEWord address, LEWord data)
        {
            tStates += 3;
            lastMemory.Address = address;
            lastMemory.Size = MemorySize.Word;
            lastMemory.Mode = MemoryMode.Write;
            Memory.WriteWord(address.ToUInt16(), data);
        }

        protected void Mem(LEWord address, Byte data)
        {
            tStates += 3;
            lastMemory.Address = address;
            lastMemory.Size = MemorySize.Byte;
            lastMemory.Mode = MemoryMode.Write;
            Memory.WriteByte(address.ToUInt16(), data);
        }

        protected void IncPC(UInt16 value)
        {
            PC += value;
        }

        protected void LD<T>(ref T to, T from)
        {
            to = from;
        }

        protected void LD(ref LEWord to, Byte from)
        {
            Mem(to, from);
        }

        protected void LD(ref Byte to, LEWord from)
        {
            to = Mem8(from);
        }

        protected void OUT(LEWord address, Byte value)
        {
            tStates += 4;
            io.WriteByte(address, value);
        }

        protected void CCF()
        {
            F = (byte)(
                (F & (FlagMask.P | FlagMask.Z | FlagMask.S)) |
                ((F & FlagMask.C) != 0 ? FlagMask.H : FlagMask.C) |
                (A & (FlagMask.B3 | FlagMask.B5)));
        }

        protected void RST(Byte address)
        {
            var ret = new LEWord(PC);
            ret.Inc();
            PUSH(ret);
            PC = address;
        }

        protected LEWord INC(LEWord reg)
        {
            reg.Inc();
            return reg;
        }

        protected Byte INC(Byte r)
        {
            r++;
            F = (byte)(
                (F & FlagMask.C) |
                (r == 0x80 ? FlagMask.V : 0) |
                ((r & 0x0F) != 0 ? 0 : FlagMask.H) |
                (r != 0 ? 0 : FlagMask.Z) |
                sz53[r]);
            return r;
        }

        protected LEWord DEC(LEWord reg)
        {
            tStates += 2;
            reg.Dec();
            return reg;
        }

        protected Byte DEC(Byte r)
        {
            F = (Byte)((F & FlagMask.C) | ((r & 0x0F) != 0 ? 0 : FlagMask.H) | FlagMask.N);
            r--;
            F |= (Byte)((r == 0x79 ? FlagMask.V : 0) | sz53[r]);
            return r;
        }

        private void LDx()
        {
            var b = Memory.ReadByte(rHL);
            Memory.WriteByte(DE, b);
            BC.Dec();
            b += A;
            F = (byte)((F & (FlagMask.C | FlagMask.Z | FlagMask.S))
                        | (BC != 0 ? FlagMask.V : 0)
                        | (b & FlagMask.B3)
                        | ((b & 0x02) != 0 ? FlagMask.B5 : 0));
        }

        protected void BIT(int b, Byte m)
        {
            var t = m & (0x01 << b);
            F = (byte)(FlagMask.H | (t == 0x80 ? FlagMask.S : 0));
            if (t == 0)
                F |= FlagMask.Z | FlagMask.V;
            else
                F |= (byte)((b == 3 ? FlagMask.B3 : 0)
                             | (b == 5 ? FlagMask.B5 : 0));
        }

        protected Byte SET(int b, Byte m)
        {
            return (Byte)(m | (Byte)(1 << b));
        }

        protected Byte RES(int b, Byte m)
        {
            return (Byte)(m & (Byte)~(1 << b));
        }

        private void CPx()
        {
            var _b = Memory.ReadByte(rHL);
            var result = (byte)(A - _b);
            var lookup = (byte)(((A & 0x08) >> 3) | (((_b) & 0x08) >> 2) | ((result & 0x08) >> 1));
            BC.Dec();
            F =
                (byte)
                    ((F & FlagMask.C) | (BC != 0 ? FlagMask.V | FlagMask.N : FlagMask.N) | hc_sub[lookup] |
                     (result != 0 ? 0 : FlagMask.Z) | (result & FlagMask.S));
            if ((F & FlagMask.H) != 0)
                result--;
            F |= (byte)((result & FlagMask.B3) | ((result & 0x02) != 0 ? FlagMask.B5 : 0));
        }

        protected void CALL(Boolean condition, LEWord address)
        {
            tStates += 17;
            if (condition)
            {
                PUSH(PC);
                PC = address;
            }
        }

        protected void PUSH(LEWord address)
        {
            tStates += 5;
            SP.Dec();
            SP.Dec();
            Mem(SP, address);
        }

        protected void JP(Boolean condition, LEWord address)
        {
            if (condition)
            {
                PC = address;
            }
        }

        protected void JR(Boolean condition, SByte e)
        {
            if (condition)
            {
                tStates += 5;
                PC = new LEWord((UInt16)(PC.ToUInt16() + e));
            }
        }

        protected LEWord POP()
        {
            tStates += 17;
            LEWord result = Memory.ReadWord(SP);
            SP.Inc();
            SP.Inc();
            return result;
        }

        protected void RET(Boolean condition)
        {
            tStates += 6;
            if (condition)
                PC = POP();
        }

        protected void CP(Byte r)
        {
            var result = (ushort)(A - r);
            var lookup = (byte)((((A & 0x88) >> 3) | ((r & 0x88) >> 2) | ((result & 0x88) >> 1)));
            F = (byte)(
                ((result & 0x100) != 0 ? FlagMask.C : (result != 0 ? 0 : FlagMask.Z)) |
                FlagMask.N |
                hc_sub[lookup & 0x07] |
                ov_sub[lookup >> 4] |
                (r & (FlagMask.B3 | FlagMask.B5)) |
                (result & FlagMask.S));
        }

        protected void AND(Byte r)
        {
            A &= r;
            F = (byte)
                (FlagMask.H |
                 sz53p[A]);
        }

        protected void OR(Byte r)
        {
            A |= r;
            F = sz53p[A];
        }

        protected void XOR(Byte r)
        {
            A ^= r;
            F = sz53p[A];
        }

        protected UInt16 Relative(Byte relative)
        {
            return (UInt16)(Int16)(PC + (SByte)relative);
        }

        protected LEWord ADC(LEWord first, LEWord second)
        {
            return ADD(first, second);
        }

        protected LEWord ADD(LEWord first, LEWord second)
        {
            var result = (uint)(first + second);
            var lookup = (byte)(
                (byte)((first & 0x0800) >> 11) |
                (byte)((second & 0x0800) >> 10) |
                (byte)((result & 0x0800) >> 9));
            F = (byte)(
                (F & (FlagMask.V | FlagMask.Z | FlagMask.S)) |
                ((result & 0x10000) != 0 ? FlagMask.C : 0) |
                (byte)((result >> 8) & (FlagMask.B3 | FlagMask.B5)) |
                hc_add[lookup]);
            return new LEWord((ushort)result);
        }

        protected void ADC(Byte n)
        {
            var result = (ushort)(A + n + ((F & FlagMask.C) != 0 ? 1 : 0));
            // Prepare the bits to perform the lookup
            var lookup = (byte)(((A & 0x88) >> 3) | ((n & 0x88) >> 2) | ((result & 0x88) >> 1));
            A = (byte)result;

            F = (byte)
                (((result & 0x100) != 0 ? FlagMask.C : 0) |
                 hc_add[lookup & 0x07] | ov_add[lookup >> 4] | sz53[A]);
        }

        protected void ADD(Byte n)
        {
            var result = (ushort)(A + n);
            var lookup = (byte)(((A & 0x88) >> 3) | (((n) & 0x88) >> 2) | ((result & 0x88) >> 1));
            A = (byte)result;
            F = (byte)
                (((result & 0x100) != 0 ? FlagMask.C : 0) |
                 hc_add[lookup & 0x07] | ov_add[lookup >> 4] | sz53[A]);
        }

        protected LEWord SBC(LEWord first, LEWord second)
        {
            var newValue = (uint)(first - second - ((F & FlagMask.C) != 0 ? 1 : 0));
            var lookup =
                (byte)
                    ((byte)((first & 0x8800) >> 11) | (byte)((second & 0x8800) >> 10) |
                     (byte)((newValue & 0x8800) >> 9));
            var result = new LEWord((ushort)newValue);
            F =
                (byte)
                    (((newValue & 0x10000) != 0 ? FlagMask.C : 0) | FlagMask.N | ov_sub[lookup >> 4] |
                     (result.High & (FlagMask.B3 | FlagMask.B5 | FlagMask.S)) | hc_sub[lookup & 0x07] |
                     (result != 0 ? 0 : FlagMask.Z));
            return result;
        }

        protected void SBC(Byte n)
        {
            var result = (ushort)(A - n - (F & FlagMask.C));
            var lookup = (byte)(((A & 0x88) >> 3) | ((n & 0x88) >> 2) | ((result & 0x88) >> 1));
            A = (byte)result;
            F =
                (byte)
                    (((result & 0x100) != 0 ? FlagMask.C : 0) | FlagMask.N | hc_sub[lookup & 0x07] | ov_sub[lookup >> 4] |
                     sz53[A]);
        }

        protected void SUB(Byte n)
        {
            var result = (ushort)(A - n);
            var lookup = (byte)(((A & 0x88) >> 3) | ((n & 0x88) >> 2) | ((result & 0x88) >> 1));
            A = (byte)result;
            F =
                (byte)
                    (((result & 0x100) != 0 ? FlagMask.C : 0) | FlagMask.N | hc_sub[lookup & 0x07] | ov_sub[lookup >> 4] |
                     sz53[A]);
        }

        protected void EX<T>(ref T first, ref T second)
        {
            var realFirst = first;
            first = second;
            second = realFirst;
        }

        protected Byte RLC(Byte r)
        {
            r = (Byte)((r << 1) | (r >> 7));
            F = (Byte)((r & FlagMask.C) | sz53p[r]);
            return r;
        }

        protected Byte RL(Byte r)
        {
            var o = r;
            r = (Byte)((r << 1) | (F & FlagMask.C));
            F = (Byte)((o >> 7) | sz53p[r]);
            return r;
        }

        protected void RLD()
        {
            var h = Mem8(rHL);
            var hLN = (Byte)(h & 15);
            var hHN = (Byte)(h & 241);
            var aLN = (Byte)(A & 15);

            h = (Byte)(aLN | (hLN << 4));
            A |= (Byte)(hHN >> 4);
            Mem(rHL, h);
        }

        protected Byte RRC(Byte r)
        {
            F = (Byte)(r & FlagMask.C);
            r = (Byte)((r >> 1) | (r << 7));
            F |= sz53p[r];
            return r;
        }

        protected Byte RR(Byte r)
        {
            var o = r;
            r = (Byte)((r >> 1) | (F << 7));
            F = (Byte)((o & FlagMask.C) | sz53p[r]);
            return r;
        }

        protected Byte SLA(Byte r)
        {
            F = (Byte)(r >> 7);
            r <<= 1;
            F |= sz53p[r];
            return r;
        }

        protected Byte SRA(Byte r)
        {
            F = (Byte)(r & FlagMask.C);
            r = (Byte)((r & 0x80) | (r >> 1));
            F |= sz53p[r];
            return r;
        }

        protected Byte SRL(Byte r)
        {
            F = (Byte)(r & FlagMask.C);
            r >>= 1;
            F |= sz53p[r];
            return r;
        }

        protected Byte SLL(Byte r)
        {
            F = (Byte)(r >> 7);
            r = (Byte)((r << 1) | 0x01);
            F |= sz53p[r];
            return r;
        }


        readonly byte[] hc_add = { 0, FlagMask.H, FlagMask.H, FlagMask.H, 0, 0, 0, FlagMask.H };
        readonly byte[] hc_sub = { 0, 0, FlagMask.H, 0, FlagMask.H, 0, FlagMask.H, FlagMask.H };

        readonly byte[] ov_add = { 0, 0, 0, FlagMask.V, FlagMask.V, 0, 0, 0 };
        readonly byte[] ov_sub = { 0, FlagMask.V, 0, 0, 0, 0, FlagMask.V, 0 };

        readonly byte[] par = new byte[0x100]; // Parity of the lookup value
        readonly byte[] sz53 = new byte[0x100]; // S, Z, 5 and 3 bits of the lookup value
        readonly byte[] sz53p = new byte[0x100]; // OR the above two tables together

        private void InitializeFlagTables()
        {
            int i;

            for (i = 0; i < 0x100; i++)
            {
                sz53[i] = (byte)(i & (FlagMask.B3 | FlagMask.B5 | FlagMask.S));

                var j = i;
                byte parity = 0;
                for (var k = 0; k < 8; k++)
                {
                    parity ^= (byte)(j & 1);
                    j >>= 1;
                }
                par[i] = (parity != 0 ? (byte)0 : FlagMask.P);
                sz53p[i] = (byte)(sz53[i] | par[i]);
            }

            sz53[0] |= FlagMask.Z;
            sz53p[0] |= FlagMask.Z;
        }
    }

    public class FlagMask
    {
        public const byte C = 0x01; // carry
        public const byte N = 0x02; // add/subtract
        public const byte P = 0x04; // parity
        public const byte V = 0x04; // overflow
        public const byte B3 = 0x08; // three bit
        public const byte H = 0x10; // half carry
        public const byte B5 = 0x20; // five bit
        public const byte Z = 0x40; // zero/equals
        public const byte S = 0x80; // sign
    }
}