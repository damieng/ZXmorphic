#region Imports

using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

using Morphic.Core.Memory;
using Morphic.Core.Processor.Z80;

using Spectrum.Custom;
using Spectrum.Memory;

#endregion

namespace Spectrum.Model
{
	public class Spectrum48K
	{
		#region Instance variables

		public Z80CPU cpu;
		private Spectrum48KULA ula;
		public Memory48K memory;

		#endregion

		#region Construction

		public Spectrum48K()
		{
			memory = new Memory48K();

			ula = new Spectrum48KULA();
			cpu = new Z80CPU(memory, ula);

			Reset();
		}

		#endregion

		#region Methods

		public void Reset()
		{
			memory.Reset();
			memory.Load(0, @"ROMs\48.rom");
			//memory.Load(16384, @"c:\temp\test.scr");

			cpu.Reset();
		}

		public void Run()
		{
			while(true)
				cpu.Cycle();
		}

		public void Cycle()
		{
			cpu.Cycle();
		}

		#endregion
	}
}