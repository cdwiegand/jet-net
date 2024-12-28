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
                        var results = JsonParser.ProcessJson(s);
                        Console.WriteLine(results.ToString());
                    }
            else
            {
                var results = JsonParser.ProcessJson(Console.OpenStandardInput());
                Console.WriteLine(results.ToString());
            }

        }
    }
}