using System;

namespace JetNet
{
	public class JsonAttribute
	{
		public JsonAttribute(string name) {
			Name = name;
		}

		public string Name { get; set; }
		
		public JsonValue? Value { get; set; }
	}
}
