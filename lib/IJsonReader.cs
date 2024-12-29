namespace JetNet
{
    public interface IJsonReader
    {
        int NextIndex { get; }
        int CurrentIndex { get; }
        bool TryPopChar(out char c, bool consumeWhitespace);
        void Rewind();
    }
}