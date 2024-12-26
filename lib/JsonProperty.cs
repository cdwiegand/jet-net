namespace JetNet
{
    public class JsonProperty : JsonValue
    {
        public JsonProperty(string name)
        {
            Name = name;
        }
        public JsonProperty(string name, object value) : this(name)
        {
            Value = JsonValue.Build(value);
        }
        public JsonProperty(string name, JsonValue value) : this(name)
        {
            Value = value;
        }

        public string Name { get; set; }

        public JsonValue? Value { get; set; }

        public override ValueTypes ValueType => ValueTypes.Property;

        public override string ToString() => ToString(JsonFormatOptions.Defaults);
        public override string ToString(JsonFormatOptions format) =>
             format.EscapeJsonString(Name, false)
             + format.FormatAfterDelimiter(":")
             + (Value ?? new JsonNullValue()).ToString(format);

        protected override JsonValue GetValue() => Value ?? throw new NullReferenceException("Value of property is null.");
    }
}
