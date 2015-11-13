using System;

using Morphic.Core.Primitive;

namespace Morphic.Core.Memory
{
    public class PagedMemory
    {
        readonly UInt32 pageSize;

        public MemoryPage[] AllPages;
        public MemoryPage[] CurrentPages;

        public PagedMemory(UInt32 totalPageCount, UInt32 pageSize, UInt32 contigPageCount)
        {
            this.pageSize = pageSize;
            AllPages = new MemoryPage[totalPageCount];
            CurrentPages = new MemoryPage[contigPageCount];

            for (var idx = 0; idx < totalPageCount; idx++)
                AllPages[idx] = new MemoryPage(pageSize, false);

            for (var idx = 0; idx < contigPageCount; idx++)
                CurrentPages[idx] = AllPages[idx];
        }

        public void WriteByte(UInt32 address, Byte data, Boolean force)
        {
            var page = (int)(address / pageSize);
            if (page >= AllPages.Length)
                return;

            if ((AllPages[page].ReadOnly) && (!force)) return;

            var pageAddress = (int)(address % pageSize);
            AllPages[page][pageAddress] = data;
        }

        public Byte ReadByte(UInt32 address)
        {
            var page = (int)(address / pageSize);
            var pageAddress = (int)(address % pageSize);
            return AllPages[page][pageAddress];
        }

        public LEWord ReadWord(UInt32 address)
        {
            return new LEWord(ReadByte(address), ReadByte(address + 1));
        }

        public void WriteWord(UInt32 address, LEWord data, Boolean force)
        {
            WriteByte(address, data.Low, force);
            WriteByte(address + 1, data.High, force);
        }
    }
}