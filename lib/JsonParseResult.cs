using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace JetNet
{
    // the only real difference between a json array and a json object is the object has named children and the array has indexed children
    public class JsonParseResult : List<JsonValue>
    {
        public override string ToString()
        {
            if (Count == 0) return "[]";
            if (Count == 1) return this[0].ToString();
            return "[ " + string.Join(", ", this.Select(p => p.ToString())) + " ]";
        }
    }
}
