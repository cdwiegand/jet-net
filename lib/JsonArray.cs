using System;
using System.Collections;

namespace JetNet
{
	// the only real difference between a json array and a json object is the object has named children and the array has indexed children
	public class JsonArray : JsonValue, IEnumerable<JsonValue>, ICollection, IEnumerable
	{
		public override ValueTypes ValueType => ValueTypes.Array;

		public List<JsonValue> Items { get; set; } = new();

        public override string ToString() => "[ "+ string.Join(", ", Items.Select(p => p.ToString())) + " ]";

        public int Count => ((ICollection)Items).Count;

        public bool IsSynchronized => ((ICollection)Items).IsSynchronized;

        public object SyncRoot => ((ICollection)Items).SyncRoot;

        public bool IsFixedSize => ((IList)Items).IsFixedSize;

        public bool IsReadOnly => ((IList)Items).IsReadOnly;

        public JsonValue? this[int index]
		{
			get => this[index];
			set => this[index] = value;
		}

		public void Add(JsonValue value) => Items.Add(value);		

        public IEnumerator<JsonValue> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        public void CopyTo(Array array, int index)=> ((ICollection)Items).CopyTo(array, index);
                
        public void Clear() => Items.Clear();

        public bool Contains(JsonValue value) => Items.Contains(value);

        public int IndexOf(JsonValue value) => Items.IndexOf(value);

        public void Remove(JsonValue value) => Items.Remove(value);
     
        public void RemoveAt(int index) => Items.RemoveAt(index);
    }
}
