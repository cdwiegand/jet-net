using System;

namespace JetNet
{
	// the only real difference between a json array and a json object is the object has named children and the array has indexed children
	public class JsonObject : JsonValue
	{
		public override ValueTypes ValueType => ValueTypes.Object;

		public List<JsonProperty> Items { get; set; } = new();

		public override string ToString() => "{ " + string.Join(", ", Items.Select(p => p.ToString())) + " }";

		public void Add(string name, JsonValue value) => Items.Add(new JsonProperty(name) { Value = value });

		public void Add(JsonProperty attr) => Items.Add(attr);
	}
}
