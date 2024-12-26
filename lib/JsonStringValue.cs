using System;

namespace JetNet
{
	public class JsonStringValue : JsonValue
	{
		public JsonStringValue() { }

		public JsonStringValue(string? value)
		{
			Value = value;
		}

		public override ValueTypes ValueType => ValueTypes.String;

		public override string ToString() => ToString(JsonFormatOptions.Defaults);
		public override string ToString(JsonFormatOptions format) => format.EscapeJsonString(Value, true);

		public string? Value { get; set; }
	}
}
