namespace JetNet
{
    // the only real difference between a json array and a json object is the object has named children and the array has indexed children
    public class JsonParseResult : List<JsonValue>
    {
        public override string ToString() => ToString(JsonFormatOptions.Defaults);
        public string ToString(JsonFormatOptions format)
        {
            format ??= JsonFormatOptions.Defaults;
            if (Count == 0) return format.AddSpacesAroundDelimiters ? "[ ]" : "[]";
            if (Count == 1) return this[0].ToString(format);
            return JsonArray.ToStringFromItems(this, format);
        }
    }
}
