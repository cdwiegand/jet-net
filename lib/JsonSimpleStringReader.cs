namespace JetNet
{
    public class JsonSimpleStringReader : IJsonReader
    {
        public JsonSimpleStringReader(string json)
        {
            OrigString = json;
        }

        public int NextIndex { get; private set; }
        public int CurrentIndex => NextIndex - 1;

        public string OrigString { get; private set; }

        public bool TryPopChar(out char c, bool consumeWhitespace)
        {
            c = '\0';
            do
            {
                if (NextIndex >= OrigString.Length) return false;
                c = OrigString[NextIndex];
                NextIndex++;
            } while (consumeWhitespace && char.IsWhiteSpace(c));
            return true;
        }

        public void Rewind()
        {
            NextIndex--;
        }
    }
}
