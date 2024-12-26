namespace JetNet
{
    public class JsonStringReaderReader : IJsonReader
    {
        public JsonStringReaderReader(StringReader s)
        {
            Stream = s;
        }

        public int NextIndex { get; private set; }
        public int CurrentIndex => NextIndex - 1;
        public int MaxBuffer { get; set; } = 1024; // 1 KB
        private string Buffer = "";
        private char? RewoundChar;
        private char? LastChar;

        private StringReader Stream { get; set; }

        public override string ToString() => Context;

        public string Context =>
            $"Index: {NextIndex - 1}\n" +
            $"Before: {PeekAround(20)}";

        private string? PeekAround(int beforeIndex)
        {
            if (beforeIndex < 0) beforeIndex = -beforeIndex;
            if (beforeIndex > Buffer.Length) return Buffer;
            return Buffer[..(Buffer.Length - beforeIndex)];
        }

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
                    if (Stream.Peek() == -1) return false;
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
