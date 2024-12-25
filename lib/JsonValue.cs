using System;

namespace JetNet
{
	public abstract class JsonValue
	{
		public enum ValueTypes {
			Array,
			Object,
			String,
		}
		
		public abstract ValueTypes ValueType { get; }
	}
}
