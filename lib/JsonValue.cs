using System;

namespace JetNet
{
    public abstract class JsonValue
    {
        public enum ValueTypes
        {
            Array,
            Object,
            String,
            Property,
        }

        public abstract ValueTypes ValueType { get; }

        public static string EscapeJsonString(string? raw, bool allowRawValues)
        {
            if (allowRawValues)
            {
                if (raw == null) return "null";
                if (raw == "true") return "true";
                if (raw == "false") return "false";
                if (decimal.TryParse(raw, out decimal dec)) return raw;
            }
            return 
                "\"" +
                raw.Replace("\\", "\\\\")
                   .Replace("\"", "\\\"")
                + "\"";
        }

        public override abstract string ToString();

        protected virtual JsonValue GetValue() => this;

        public virtual JsonArray AsArray() =>
            GetValue().ValueType == ValueTypes.Array && GetValue() is JsonArray ret
            ? ret
            : throw new InvalidCastException("Type is " + ValueType + ", expected JsonArray.");

        public virtual JsonObject AsObject() =>
            GetValue().ValueType == ValueTypes.Object && GetValue() is JsonObject ret
            ? ret
            : throw new InvalidCastException("Type is " + ValueType + ", expected JsonObject.");

        public virtual JsonString AsString() =>
            GetValue().ValueType == ValueTypes.String && GetValue() is JsonString ret
            ? ret
            : throw new InvalidCastException("Type is " + ValueType + ", expected JsonString.");
    }
}
