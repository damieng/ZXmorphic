using Morphic.Core.CPU.Z80;
using Spectrum.Custom;
using Spectrum.Memory;

namespace Spectrum.Model
{
    public class Spectrum48K
    {
        public readonly Z80CPU Cpu;
        public readonly Memory48K Memory;
        public readonly Spectrum48KULA Ula;

        public Spectrum48K()
        {
            Memory = new Memory48K();
            Ula = new Spectrum48KULA();
            Cpu = new Z80CPU(Memory, Ula);

            Reset();
        }

        public void Reset()
        {
            Ula.Reset();
            Memory.Reset();
            Memory.Load(0, @"ROMs\48.rom");
            Cpu.Reset();
        }

        public void Run()
        {
            while (true)
                Cpu.Cycle();
        }

        public void Cycle()
        {
            Cpu.Cycle();
        }
    }
}