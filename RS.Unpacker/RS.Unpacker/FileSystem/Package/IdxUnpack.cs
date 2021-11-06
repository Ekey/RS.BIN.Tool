using System;
using System.IO;
using System.Collections.Generic;

namespace RS.Unpacker
{
    class IdxUnpack
    {
        static List<IdxEntry> m_EntryTable = new List<IdxEntry>();

        public static void iDoIt(String m_IndexFile, String m_DstFolder)
        {
            IdxHashList.iLoadProject();
            using (FileStream TIdxStream = File.OpenRead(m_IndexFile))
            {
                var lpHeader = TIdxStream.ReadBytes(16);
                var m_Header = new IdxHeader();

                using (var THeaderReader = new MemoryStream(lpHeader))
                {
                    m_Header.dwMagic = THeaderReader.ReadUInt32();
                    m_Header.dwVersion = THeaderReader.ReadInt32();
                    m_Header.dwTotalFiles = THeaderReader.ReadInt32();
                    m_Header.dwAligment = THeaderReader.ReadInt32();

                    if (m_Header.dwMagic != 0x52444854)
                    {
                        throw new Exception("[ERROR]: Invalid magic of IDX file!");
                    }

                    if (m_Header.dwVersion != 8)
                    {
                        throw new Exception("[ERROR]: Invalid version of IDX file => " + m_Header.dwVersion.ToString() + " expected 8");
                    }

                    if (m_Header.dwAligment != 16)
                    {
                        throw new Exception("[ERROR]: Invalid aligment of IDX file => " + m_Header.dwAligment.ToString() + " expected 16");
                    }

                    THeaderReader.Dispose();
                }

                var lpEntryHeader = TIdxStream.ReadBytes(8);
                var m_EntryHeader = new IdxEntryHeader();

                using (var THeaderReader = new MemoryStream(lpEntryHeader))
                {
                    m_EntryHeader.dwMagic = THeaderReader.ReadUInt32();
                    m_EntryHeader.dwTableSize = THeaderReader.ReadInt32();

                    if (m_EntryHeader.dwMagic != 0x4C494654)
                    {
                        throw new Exception("[ERROR]: Invalid magic of entry table!");
                    }
                }

                m_EntryTable.Clear();
                var lpTable = TIdxStream.ReadBytes(m_EntryHeader.dwTableSize);

                using (var TEntryReader = new MemoryStream(lpTable))
                {
                    for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                    {
                        UInt32 dwHash = TEntryReader.ReadUInt32();
                        UInt32 dwOffset = TEntryReader.ReadUInt32();
                        Int32 dwSize = TEntryReader.ReadInt32();

                        var TEntry = new IdxEntry
                        {
                            dwHash = dwHash,
                            dwOffset = dwOffset * (UInt32)m_Header.dwAligment,
                            dwSize = dwSize,
                        };

                        m_EntryTable.Add(TEntry);
                    }

                    TEntryReader.Dispose();
                }

                TIdxStream.Dispose();
            }

            using (FileStream TBinStream = File.OpenRead(m_IndexFile.Replace(".idx", ".bin")))
            {
                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FileName = IdxHashList.iGetNameFromHashList(m_Entry.dwHash);
                    String m_FullPath = m_DstFolder + m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TBinStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
                    var lpBuffer = TBinStream.ReadBytes(m_Entry.dwSize);

                    m_FullPath = IdxUtils.iDetectFileType(m_FullPath, lpBuffer);

                    File.WriteAllBytes(m_FullPath, lpBuffer);
                }
            }
        }
    }
}
