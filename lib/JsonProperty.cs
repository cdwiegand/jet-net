using System;

namespace JetNet
{
    public class JsonProperty : JsonValue
    {
        public JsonProperty(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public JsonValue? Value { get; set; }

        public override ValueTypes ValueType => ValueTypes.Property;

        public override string ToString() =>
             "\"" + JsonValue.EscapeJsonString(Name) + "\": " + (Value == null ? "null" : Value.ToString());

        protected override JsonValue GetValue() => Value ?? throw new NullReferenceException("Value of property is null.");
    }
}
