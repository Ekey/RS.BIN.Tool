using System;

namespace RS.Packer
{
    class IdxEntry
    {
        public UInt32 dwHash { get; set; }
        public UInt32 dwOffset { get; set; }
        public Int32 dwSize { get; set; }
    }
}
