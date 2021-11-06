using System;

namespace RS.Unpacker
{
    class IdxEntry
    {
        public UInt32 dwHash { get; set; }
        public UInt32 dwOffset { get; set; } // * dwAligment
        public Int32 dwSize { get; set; }
    }
}
