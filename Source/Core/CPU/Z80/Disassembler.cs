using System;
using System.Collections.Generic;
using Morphic.Core.Primitive;

namespace Morphic.Core.CPU.Z80
{
    public class Disassembler
    {
        const string HexByte = "${0:X2}";
        const string HexWord = "${0:X4}";

        readonly Dictionary<UInt16, Block> disassembly = new Dictionary<UInt16, Block>();
        readonly Z80CPU z80;

        private ManipulateDecoded manipulateDecoded;

        public Disassembler(Z80CPU z80)
        {
            this.z80 = z80;
        }

        public List<Block> Disassemble(UInt16 address)
        {
            var result = new List<Block>();
            var valid = true;

            while (valid)
            {
                if (!disassembly.ContainsKey(address))
                    disassembly[address] = Decode(address);
                result.Add(disassembly[address]);
                var text = disassembly[address].Disassembly;
                address += disassembly[address].Length;

                // Stop debugging when an uncondition flow control change
                if (text == "JP" || text == "RET")
                    valid = false;
            }

            return result;
        }

        private Block Decode(UInt16 address)
        {
            var result = new Block(address, 0, "");
            var state = new State { Address = address };
            var op = DecodeOp(state);
            result.Disassembly = DecodeText(state, op);
            result.Length = (UInt16)(state.Address - result.Address);
            return result;
        }

        private String DecodeText(State state, Z80CPU.Op op)
        {
            if (op == null)
                return String.Format("DEFB " + HexByte, z80.Memory.ReadByte(state.Address));

            var text = op.Name;
            if (manipulateDecoded != null)
            {
                text = manipulateDecoded.Invoke(text);
                manipulateDecoded = null;
            }

            if (text.Contains("x"))
            {
                var x = state.IndexerOffset ?? (SByte)z80.Memory.ReadByte(state.Address++);
                text = text.Replace("x", x.ToString("+;-;") + String.Format(HexByte, x));
            }

            if (text.Contains("e"))
            {
                var e = (SByte)(z80.Memory.ReadByte(state.Address++));
                text = text.Replace("e", "$" + new LEWord((UInt16)(state.Address + e)));
                // + " (" + e.ToString("+0;-0;0") + ")");
            }

            if (text.Contains("nn"))
            {
                var nn = z80.Memory.ReadWord(state.Address);
                state.Address += 2;
                text = text.Replace("nn", String.Format(HexWord, nn));
            }

            if (text.Contains("n"))
            {
                var n = z80.Memory.ReadByte(state.Address++);
                text = text.Replace("n", String.Format(HexByte, n));
            }

            return text;
        }

        private Z80CPU.Op DecodeOp(State state)
        {
            while (true)
            {
                var b = z80.Memory.ReadByte(state.Address++);
                switch (b)
                {
                    case 0xDD:
                        manipulateDecoded = input => input.Replace("(HL)", "(IXx)").Replace("HL", "IX");
                        state.IndexerRegister = Z80CPU.IndexerRegister.IX;
                        continue;
                    case 0xFD:
                        manipulateDecoded = input => input.Replace("(HL)", "(IYx)").Replace("HL", "IY");
                        state.IndexerRegister = Z80CPU.IndexerRegister.IY;
                        continue;
                    case 0xCB:
                        if (state.IndexerRegister != Z80CPU.IndexerRegister.HL)
                            state.IndexerOffset = (SByte) z80.Memory.ReadByte(state.Address++);
                        return z80.opCB[z80.Memory.ReadByte(state.Address++)];
                    case 0xED:
                        return z80.opED[z80.Memory.ReadByte(state.Address++)];
                    default:
                        return z80.op[b];
                }
            }
        }

        public class Block
        {
            public UInt16 Address;
            public String Disassembly;
            public UInt16 Length;

            public Block(UInt16 address, UInt16 length, String text)
            {
                Address = address;
                Length = length;
                Disassembly = text;
            }
        }

        delegate string ManipulateDecoded(string input);

        private class State
        {
            public UInt16 Address;
            public SByte? IndexerOffset;
            public Z80CPU.IndexerRegister IndexerRegister = Z80CPU.IndexerRegister.HL;
            public Z80CPU.InstructionSets InstructionSet = Z80CPU.InstructionSets.Normal;
        }
    }
}