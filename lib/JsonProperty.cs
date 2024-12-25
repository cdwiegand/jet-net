using System;

namespace JetNet
{
	public class JsonProperty
	{
		public JsonProperty(string name)
		{
			Name = name;
		}

		public string Name { get; set; }

		public JsonValue? Value { get; set; }

		public override string ToString() =>
			 "\"" + JsonValue.EscapeJsonString(Name) + "\": " + (Value == null ? "null" : Value.ToString());
	}
}
