using System;
using System.IO;

namespace RS.Unpacker
{
    class IdxUtils
    {
        public static String iDetectFileType(String m_FileName, Byte[] lpBuffer)
        {
            if (m_FileName.Contains(@"__Unknown"))
            {
                if (lpBuffer.Length > 0)
                {
                    UInt32 dwMagic = BitConverter.ToUInt32(lpBuffer, 0);
                    switch (dwMagic)
                    {
                        case 0x38464947: return m_FileName + ".gif";
                        case 0x43494453: return m_FileName + ".sdic";
                        case 0x474E5089: return m_FileName + ".png";
                        case 0x44485441: return m_FileName + ".athd";
                        case 0x4448444D: return m_FileName + ".mdhd";
                        case 0x44484F4D: return m_FileName + ".mohd";
                        case 0x52444846: return m_FileName + ".fhdr";
                        case 0x5244484D: return m_FileName + ".mhdr";
                        case 0x58545653: return m_FileName + ".ssb";
                        case 0xE0FFD8FF: return m_FileName + ".jpg";
                    }
                }
                return m_FileName;
            }
            else
            {
                return m_FileName;
            }
        }
    }
}
