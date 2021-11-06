using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace RS.Packer
{
    class IdxPack
    {
        static List<IdxEntry> m_EntryTable = new List<IdxEntry>();

        public static UInt32 iAlignUInt32(UInt32 dwValue, UInt32 dwAlignSize)
        {
            if (dwValue == 0)
            {
                return dwValue;
            }

            return dwValue + ((dwAlignSize - (dwValue % dwAlignSize)) % dwAlignSize);
        }

        public static void iDoIt(String m_IndexFile, String m_SrcFolder)
        {
            String m_BinFile = Path.GetDirectoryName(m_IndexFile) + @"\" + Path.GetFileNameWithoutExtension(m_IndexFile) + ".bin";
            var m_Header = new IdxHeader();

            m_Header.dwMagic = 0x52444854;
            m_Header.dwVersion = 8;
            m_Header.dwTotalFiles = Directory.GetFiles(m_SrcFolder, "*.*", SearchOption.AllDirectories).Length;
            m_Header.dwAligment = 16;

            var m_EntryHeader = new IdxEntryHeader();

            m_EntryHeader.dwMagic = 0x4C494654;
            m_EntryHeader.dwTableSize = m_Header.dwTotalFiles * 12;

            using (BinaryWriter TBinWriter = new BinaryWriter(File.Open(m_BinFile, FileMode.Create)))
            {
                Byte[] lpAligned = Enumerable.Repeat((Byte)0, m_Header.dwAligment).ToArray();
                TBinWriter.Write(lpAligned);

                foreach (String m_File in Directory.GetFiles(m_SrcFolder, "*.*", SearchOption.AllDirectories))
                {
                    String m_FileName = m_File.Replace(m_SrcFolder, "").Replace(@"\", "/");
                    var m_Entry = new IdxEntry();

                    Console.WriteLine("[PACKING]: {0}", m_FileName);

                    if (!m_File.Contains("__Unknown"))
                    {
                        m_Entry.dwHash = IdxHash.iGetHash(m_FileName);
                    }
                    else
                    {
                        m_Entry.dwHash = Convert.ToUInt32(Path.GetFileNameWithoutExtension(m_FileName), 16);
                    }

                    var lpBuffer = File.ReadAllBytes(m_File);
                    UInt32 dwAlignedSize = iAlignUInt32((UInt32)lpBuffer.Length, (UInt32)m_Header.dwAligment);
                    Array.Resize(ref lpBuffer, (Int32)dwAlignedSize);

                    m_Entry.dwOffset = (UInt32)TBinWriter.BaseStream.Position;
                    m_Entry.dwSize = lpBuffer.Length;

                    TBinWriter.Write(lpBuffer);

                    m_EntryTable.Add(m_Entry);
                }
                TBinWriter.Dispose();
            }

            using (BinaryWriter TIdxWriter = new BinaryWriter(File.Open(m_IndexFile, FileMode.Create)))
            {
                TIdxWriter.Write(m_Header.dwMagic);
                TIdxWriter.Write(m_Header.dwVersion);
                TIdxWriter.Write(m_Header.dwTotalFiles);
                TIdxWriter.Write(m_Header.dwAligment);

                TIdxWriter.Write(m_EntryHeader.dwMagic);
                TIdxWriter.Write(m_EntryHeader.dwTableSize);

                m_EntryTable = m_EntryTable.OrderBy(x => x.dwHash).ToList();

                foreach (var m_Entry in m_EntryTable)
                {
                    TIdxWriter.Write(m_Entry.dwHash);
                    TIdxWriter.Write(m_Entry.dwOffset / 16);
                    TIdxWriter.Write(m_Entry.dwSize);
                }

                TIdxWriter.Dispose();
            }
        }
    }
}
