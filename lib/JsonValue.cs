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
		}

		public abstract ValueTypes ValueType { get; }

		public static string EscapeJsonString(string raw) =>
			raw.Replace("\\", "\\\\")
			   .Replace("\"", "\\\"");

		public override abstract string ToString();

		public JsonArray AsArray() => ValueType == ValueTypes.Array && this is JsonArray ret ? ret : throw new InvalidCastException("Type is " + ValueType + ", expected JsonArray.");
		public JsonObject AsObject() => ValueType == ValueTypes.Object && this is JsonObject ret ? ret : throw new InvalidCastException("Type is " + ValueType + ", expected JsonObject.");
		public JsonString AsString() => ValueType == ValueTypes.String && this is JsonString ret ? ret : throw new InvalidCastException("Type is " + ValueType + ", expected JsonString.");
	}
}
