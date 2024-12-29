namespace JetNet
{
    public class JsonStreamReader : IJsonReader
    {
        public JsonStreamReader(Stream s)
        {
            Stream = new StreamReader(s);
        }
        public JsonStreamReader(StreamReader s)
        {
            Stream = s;
        }

        public int NextIndex { get; private set; }
        public int CurrentIndex => NextIndex - 1;
        private char? RewoundChar;
        private char? LastChar;

        private StreamReader Stream { get; set; }

        public bool TryPopChar(out char c, bool consumeWhitespace)
        {
            c = '\0';
            do
            {
                if (RewoundChar != null)
                {
                    c = RewoundChar.Value;
                    RewoundChar = null; // used up
                }
                else
                {
                    char[] chars = new char[1];
                    if (!Stream.BaseStream.CanRead || Stream.EndOfStream) return false;
                    Stream.Read(chars, 0, 1);
                    NextIndex++;
                    LastChar = c = chars[0];
                }
            } while (consumeWhitespace && char.IsWhiteSpace(c));
            return true;
        }

        public void Rewind()
        {
            RewoundChar = LastChar; // can't go back more than one..
        }
    }
}
