using System;

namespace RS.Unpacker
{
    class IdxHeader
    {
        public UInt32 dwMagic { get; set; } // 0x52444854 (THDR)
        public Int32 dwVersion { get; set; } // 8
        public Int32 dwTotalFiles { get; set; }
        public Int32 dwAligment { get; set; } // 16
    }

    class IdxEntryHeader
    {
        public UInt32 dwMagic { get; set; } // 0x4C494654 (TFIL)
        public Int32 dwTableSize { get; set; }
    }
}
