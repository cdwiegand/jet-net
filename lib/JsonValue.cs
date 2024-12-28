using System.Collections;

namespace JetNet
{
    public abstract class JsonValue
    {
        public enum ValueTypes
        {
            Array,
            Object,
            String,
            Primitive,
            Null,
        }

        public abstract ValueTypes ValueType { get; }

        public override string ToString() => ToString(JsonFormatOptions.Defaults);
        public abstract string ToString(JsonFormatOptions format);

        protected virtual JsonValue GetValue() => this;

        public static JsonValue Build(object? item)
        {
            if (item == null) return new JsonNullValue();
            if (item is JsonValue jv) return jv;

            // ok, convert to json type
            if (item.GetType() == typeof(string)) return BuildStringOrPrimitive((string)item);
            if (item.GetType().IsPrimitive) return new JsonPrimitiveValue(item);
            if (item.GetType().IsArray)
            {
                JsonArray ret = new JsonArray();
                Array.ForEach<object>((object[])item, v => ret.Add(Build(v)));
                return ret;
            }
            if (item.GetType().GetInterfaces().Any(p => p == typeof(ICollection)))
            {
                JsonArray ret = new JsonArray();
                ICollection icol = (ICollection)item;
                Array.ForEach<object>((object[])item, v => ret.Add(Build(v)));
                return ret;
            }
            return BuildStringOrPrimitive(item.ToString()??"");
        }
        internal static JsonValue BuildStringOrPrimitive(string value)
        {
            if (value == null) return new JsonNullValue();
            if (value == "") return new JsonStringValue();
            string valL = value.ToLower();
            if (valL == "true") return new JsonPrimitiveValue(true);
            if (valL == "false") return new JsonPrimitiveValue(false);
            if (decimal.TryParse(valL, out decimal valD)) return new JsonPrimitiveValue(valD);
            return new JsonStringValue(value);
        }

        public void IfArray(Action<JsonArray> action)
        {
            var val = AsArray(false);
            if (val != null) action(val);
        }

        public JsonArray? AsArray(bool throwExceptionIfFail = true)
        {
            var val = GetValue();
            if (val.ValueType == ValueTypes.Array && val is JsonArray ret) return ret;
            if (throwExceptionIfFail)
                throw new InvalidCastException("Type is " + val.ValueType + ", expected Array.");
            else return null;
        }

        public void IfObject(Action<JsonObject> action)
        {
            var val = AsObject(false);
            if (val != null) action(val);
        }

        public JsonObject? AsObject(bool throwExceptionIfFail = true)
        {
            var val = GetValue();
            if (val.ValueType == ValueTypes.Object && val is JsonObject ret) return ret;
            if (throwExceptionIfFail)
                throw new InvalidCastException("Type is " + val.ValueType + ", expected Object.");
            else return null;
        }

        public JsonStringValue? AsString(bool throwExceptionIfFail = true)
        {
            var val = GetValue();
            if (val.ValueType == ValueTypes.Null && val is JsonNullValue retN) return new JsonStringValue(null);
            if (val.ValueType == ValueTypes.Primitive && val is JsonStringValue retP) return new JsonStringValue(retP.Value);
            if (val.ValueType == ValueTypes.String && val is JsonStringValue retS) return retS;
            if (throwExceptionIfFail)
                throw new InvalidCastException("Type is " + val.ValueType + ", expected String.");
            else return null;
        }

        public T? AsPrimitive<T>()
        {
            var value = GetValue();
            if (value == null) throw new NullReferenceException();
            if (value.ValueType != ValueTypes.Primitive) throw new InvalidCastException("Type is " + value.ValueType + ", expected Primitive.");

            JsonPrimitiveValue valP = value as JsonPrimitiveValue ?? throw new InvalidCastException("Could not coerce type to JsonPrimitiveValue.");
            if (valP.Value == null) return default(T);
            return (T?)Convert.ChangeType(valP.Value, typeof(T));
        }
    }
}
