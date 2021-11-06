using System;

namespace RS.Packer
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Romancing SaGa BIN Packer");
            Console.WriteLine("(c) 2021 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    RS.Packer <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Destination IDX file");
                Console.WriteLine("    m_Directory - Source directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    RS.Packer D:\\data_my.idx D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_IdxFile = args[0];
            String m_Input = Utils.iCheckArgumentsPath(args[1]);

            IdxPack.iDoIt(m_IdxFile, m_Input);
        }
    }
}
