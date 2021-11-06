using System;
using System.IO;

namespace RS.Unpacker
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Romancing SaGa BIN Unpacker");
            Console.WriteLine("(c) 2021 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    RS.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of IDX index file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    RS.Unpacker E:\\Games\\RS3\\rs3_Data\\StreamingAssets\\data.idx D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_IdxFile = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_IdxFile))
            {
                Utils.iSetError("[ERROR]: Input IDX file -> " + m_IdxFile + " <- does not exist");
                return;
            }

            IdxUnpack.iDoIt(m_IdxFile, m_Output);
        }
    }
}
