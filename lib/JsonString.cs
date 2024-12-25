using System;

namespace JetNet
{
	public class JsonString : JsonValue
	{
		public JsonString() { }
		
		public JsonString(string value)
		{
			Value = value;
		}

		public override ValueTypes ValueType => ValueTypes.String;

		public string? Value { get; set; }
	}
}
