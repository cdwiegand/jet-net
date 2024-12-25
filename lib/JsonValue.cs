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

        public static string EscapeJsonString(string raw) =>
            "\"" +
            raw.Replace("\\", "\\\\")
               .Replace("\"", "\\\"")
            + "\"";

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
