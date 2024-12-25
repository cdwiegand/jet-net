using System;
using System.Collections;
using System.Diagnostics.Contracts;

namespace JetNet
{
    // the only real difference between a json array and a json object is the object has named children and the array has indexed children
    public class JsonArray : JsonValue, IList<JsonValue>, IEnumerable<JsonValue>, ICollection<JsonValue>, IEnumerable
    {
        public override ValueTypes ValueType => ValueTypes.Array;

        public List<JsonValue> Items { get; set; } = new();

        public override string ToString() => "[ " + string.Join(", ", Items.Select(p => p.ToString())) + " ]";

        public int Count => Items.Count;

        public bool IsReadOnly => false;

        public JsonValue this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }

        public void Add(JsonValue value) => Items.Add(value);

        public void Insert(int index, JsonValue item) => Items.Insert(index, item);

        public IEnumerator<JsonValue> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        public void CopyTo(Array array, int index) => ((ICollection)Items).CopyTo(array, index);

        public void Clear() => Items.Clear();

        public bool Contains(JsonValue value) => Items.Contains(value);

        public int IndexOf(JsonValue value) => Items.IndexOf(value);

        public bool Remove(JsonValue value) => Items.Remove(value);

        public void RemoveAt(int index) => Items.RemoveAt(index);

        public void CopyTo(JsonValue[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);
    }
}
