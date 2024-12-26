using JetNet;

namespace JetLib
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
                // file(s) to read and parse
                foreach (string arg in args)
                    using (Stream s = System.IO.File.Open(arg, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        var parser = new JsonParser(s);
                        var results = parser.ProcessJson();
                        Console.WriteLine(results.ToString());
                    }
            else
            {
                var parser = new JsonParser(Console.OpenStandardInput());
                var results = parser.ProcessJson();
                Console.WriteLine(results.ToString());
            }

        }
    }
}