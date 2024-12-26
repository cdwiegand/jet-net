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
            Property,
            Primitive,
            Null,
        }

        public abstract ValueTypes ValueType { get; }

        public override abstract string ToString();
        public abstract string ToString(JsonFormatOptions format);

        protected virtual JsonValue GetValue() => this;

        public static JsonValue Build(object? item)
        {
            if (item == null) return new JsonNullValue();
            if (item is JsonValue jv) return jv;

            // ok, convert to json type
            if (item.GetType() == typeof(string)) return new JsonStringValue((string)item);
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
            return new JsonStringValue(item?.ToString());
        }

        public virtual JsonArray AsArray() =>
            GetValue().ValueType == ValueTypes.Array && GetValue() is JsonArray ret
            ? ret
            : throw new InvalidCastException("Type is " + ValueType + ", expected JsonArray.");

        public virtual JsonObject AsObject() =>
            GetValue().ValueType == ValueTypes.Object && GetValue() is JsonObject ret
            ? ret
            : throw new InvalidCastException("Type is " + ValueType + ", expected JsonObject.");

        public virtual JsonStringValue AsString()
        {
            var valType = GetValue().ValueType;
            if (valType == ValueTypes.Null && GetValue() is JsonNullValue retN) return new JsonStringValue(null);
            if (valType == ValueTypes.Primitive && GetValue() is JsonStringValue retP) return new JsonStringValue(retP.Value);
            if (valType == ValueTypes.String && GetValue() is JsonStringValue retS) return retS;
            throw new InvalidCastException("Type is " + ValueType + ", expected JsonString.");
        }
    }
}
