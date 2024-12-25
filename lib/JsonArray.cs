using System;

namespace JetNet
{
	// the only real difference between a json array and a json object is the object has named children and the array has indexed children
	public class JsonArray : JsonValue
	{
        public override ValueTypes ValueType => ValueTypes.Array;
		
		public List<JsonValue> Children { get; set; } = new();
		
		public void Add(JsonValue value)
		{
			Children.Add(value);
		}
	}
}
