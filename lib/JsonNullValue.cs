namespace JetNet
{
    public class JsonNullValue : JsonValue
    {
        public override ValueTypes ValueType => ValueTypes.Null;

        public override string ToString() => ToString(JsonFormatOptions.Defaults);
        public override string ToString(JsonFormatOptions format) => format.EscapeJsonString(null, true);
    }
}
