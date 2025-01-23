using System.Collections.Generic;

namespace EEPROMProgrammer
{
    public class MemorySegments : List<MemorySegment>
    { }

    public class MemorySegment
    {
        public int StartAddress { get; }

        public int EndAddress => StartAddress + Data.Length - 1;

        public byte[] Data { get; }

        public MemorySegment(int address, byte[] data)
        {
            StartAddress = address;
            Data = data;
        }
    }

}
