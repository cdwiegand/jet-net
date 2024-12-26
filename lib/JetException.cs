namespace JetNet
{
    [Serializable]
    public class JetException : Exception
    {
        public JetException() { }
        public JetException(string message) : base(message) { }
        public JetException(string message, Exception inner) : base(message, inner) { }
    }
}
