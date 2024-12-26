namespace JetNet
{
    // the only real difference between a json array and a json object is the object has named children and the array has indexed children
    public class JsonObject : JsonValue
    {
        public override ValueTypes ValueType => ValueTypes.Object;

        public List<JsonProperty> Items { get; set; } = new();

        public JsonValue? this[string key]
        {
            get => Items.FirstOrDefault(p => p.Name == key)?.Value;
            set
            {
                var found = Items.FirstOrDefault(p => p.Name == key);
                if (found == null) Items.Add(new JsonProperty(key, value));
                else found.Value = value;
            }
        }

        public override string ToString() => ToString(JsonFormatOptions.Defaults);
        public override string ToString(JsonFormatOptions format)
        {
            format ??= JsonFormatOptions.Defaults;
            return format.FormatAfterDelimiter("{") +
                string.Join(format.FormatAfterDelimiter(","), Items.Select(p => p.ToString(format))) +
                format.FormatBeforeDelimiter("}");
        }

        public void Add(string name, object value) => Add(new JsonProperty(name) { Value = JsonValue.Build(value) });
        public void Add(string name, JsonValue value) => Add(new JsonProperty(name) { Value = value });
        public void Add(JsonProperty attr) => Items.Add(attr);

        public JsonObject AddMore(string name, object? value) => AddMore(name, JsonValue.Build(value));
        public JsonObject AddMore(string name, JsonValue value) => AddMore(new JsonProperty(name) { Value = value });
        public JsonObject AddMore(JsonProperty attr)
        {
            Add(attr);
            return this;
        }
    }
}
