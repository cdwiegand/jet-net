using JetLib;

namespace JetLib {
  public static class Program {
    public static void Main(string[] args) {
		/*
		JsonStreamReader streamReader = new JsonStreamReader("");
		JsonParser p = new JsonParser("");
		if (args.Length > 0) {
			// file(s) to read and parse
			foreach (string arg in args) {
				Task.Run(async() => {
					using (Stream s = System.IO.File.Open(arg, FileMode.Open, FileAccess.Read)) {
						while (s.CanRead) {
							string? line = await s.ReadLineAsync();
							if (line != null) {
								p.Append(line+"\n");
								p.ProcessJson();
							}
						}
					}
				}).GetAwaiter().GetResult();
			}
		}
		*/
	}
  }
}