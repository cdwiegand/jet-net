using System;

namespace JetNet
{
	// the only real difference between a json array and a json object is the object has named children and the array has indexed children
	public class JsonObject : JsonValue
	{		
		public override ValueTypes ValueType => ValueTypes.Object;

		public List<JsonAttribute> Children { get; set; } = new();

		public void Add(string name, JsonValue value)
		{
			Children.Add(new JsonAttribute(name) { Value = value });
		}

		public void Add(JsonAttribute attr)
		{
			Children.Add(attr);
		}
	}
}
