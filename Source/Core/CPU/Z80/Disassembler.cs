using System;
using System.Collections.Generic;
using System.Text;

using Morphic.Core.Bus;
using Morphic.Core.Memory;
using Morphic.Core.Primitive;
using Morphic.Core.Processor.Z80;

namespace Morphic.Core.Processor.Z80
{
	public class Disassembler
	{
		const String HexByte = "${0:X2}";
		const String HexWord = "${0:X4}";

		public class Block
		{
			public UInt16 Address;
			public UInt16 Length;
			public String Disassembly;

			public Block(UInt16 address, UInt16 length, String text) {
				Address = address;
				Length = length;
				Disassembly = text;
			}
		}

		private Z80CPU z80;
		private Dictionary<UInt16, Block> disassembly = new Dictionary<UInt16, Block>();
		private delegate string ManipulateDecoded(string input);
		private ManipulateDecoded manipulateDecoded;

		public Disassembler(Z80CPU z80)
		{
			this.z80 = z80;
		}

		public List<Block> Disassemble(UInt16 address)
		{
			List<Block> result = new List<Block>();
			Boolean valid = true;

			while (valid) {
				if (!disassembly.ContainsKey(address))
					disassembly[address] = Decode(address);
				result.Add(disassembly[address]);
				string text = disassembly[address].Disassembly;
				address += disassembly[address].Length;

				// Stop debugging when out of range or an uncondition flow control change
				if ((address > UInt16.MaxValue) || text == "JP"  || text == "RET")
					valid = false;
			}

			return result;
		}

		private Block Decode(UInt16 address)
		{
			Block result = new Block(address, 0, "");
            var state = new State() { address = address };
			var op = DecodeOp(state);
            result.Disassembly = DecodeText(state, op);
			result.Length = (UInt16) (state.address - result.Address);
			return result;
		}

        private String DecodeText(State state, Z80CPU.Op op)
        {
            String text = "";
            if (op == null)
                return String.Format("DEFB " + HexByte, z80.Memory.ReadByte(state.address));

            text = op.Name;
			if (manipulateDecoded != null) {
				text = manipulateDecoded.Invoke(text);
				manipulateDecoded = null;
			}

            if (text.Contains("x"))
            {
                SByte x = (state.indexerOffset.HasValue) ? state.indexerOffset.Value : (SByte)z80.Memory.ReadByte(state.address++);
                text = text.Replace("x", x.ToString("+;-;") + String.Format(HexByte, x));
            }

            if (text.Contains("e"))
            {
                SByte e = (SByte)(z80.Memory.ReadByte(state.address++));
                text = text.Replace("e", "$" + new LEWord((UInt16)(state.address + e)).ToString());// + " (" + e.ToString("+0;-0;0") + ")");
            }

            if (text.Contains("nn"))
            {
                UInt16 nn = z80.Memory.ReadWord(state.address);
                state.address += 2;
                text = text.Replace("nn", String.Format(HexWord, nn));
            }

            if (text.Contains("n"))
            {
                Byte n = z80.Memory.ReadByte(state.address++);
                text = text.Replace("n", String.Format(HexByte, n));
            }

            return text;
        }

        private class State
        {
            public UInt16 address;
            public Z80CPU.InstructionSets instructionSet = Z80CPU.InstructionSets.Normal;
            public Z80CPU.IndexerRegister indexerRegister = Z80CPU.IndexerRegister.BasicHL;
            public SByte? indexerOffset = null;
        }

		private Z80CPU.Op DecodeOp(State state)
		{
			Byte b = z80.Memory.ReadByte(state.address++);
			switch (b) {
				case 0xDD:
					manipulateDecoded = delegate(string input) { return input.Replace("(HL)", "(IXx)").Replace("HL", "IX"); };
                    state.indexerRegister = Z80CPU.IndexerRegister.IndexX;
					return DecodeOp(state);
				case 0xFD:
                    manipulateDecoded = delegate(string input) { return input.Replace("(HL)", "(IYx)").Replace("HL", "IY"); };
                    state.indexerRegister = Z80CPU.IndexerRegister.IndexY;
                    return DecodeOp(state);
				case 0xCB:
                    if (state.indexerRegister != Z80CPU.IndexerRegister.BasicHL)
                        state.indexerOffset = (SByte)z80.Memory.ReadByte(state.address++);
					return z80.opCB[z80.Memory.ReadByte(state.address++)];
				case 0xED:
					return z80.opED[z80.Memory.ReadByte(state.address++)];
				default:
					return z80.op[b];
			}
		}
	}
}