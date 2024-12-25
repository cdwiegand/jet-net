using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace JetNet
{
	// the only real difference between a json array and a json object is the object has named children and the array has indexed children
	public class JsonParseResult : List<JsonValue>
	{
		public override string ToString() =>
			Count == 0 ? "[]" : Count == 1 ? this[0].ToString() : "[ " + string.Join(", ", this.Select(p => p.ToString())) + " ]";
	}
}
