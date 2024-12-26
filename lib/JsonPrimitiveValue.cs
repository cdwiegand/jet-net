namespace JetNet
{
    public class JsonPrimitiveValue : JsonValue
    {
        public JsonPrimitiveValue(object? value)
        {
            Value = value;
        }

        public override ValueTypes ValueType => ValueTypes.String;

        public override string ToString() => ToString(JsonFormatOptions.Defaults);
        public override string ToString(JsonFormatOptions format)
        {
            if (Value == null) return format.EscapeJsonString(null, true);
            if (Value is bool bValue) return format.EscapeJsonString(bValue ? "true" : "false", true); // javascript boolean values are lowercase, can't use .ToString..
            return format.EscapeJsonString(Value?.ToString(), true);
        }

        public object? Value { get; set; }
    }
}
