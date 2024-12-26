using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Text;

namespace JetNet
{
	public class JsonStringReader : IJsonReader
	{
		public JsonStringReader(string json)
		{
			OrigString = json;
		}

		public int NextIndex { get; private set; }
		public int CurrentIndex => NextIndex - 1;

		public string OrigString { get; private set; }

		public override string ToString() => Context;

		public void Append(string json)
		{
			OrigString += json;
		}

		public string Context =>
			$"Index: {NextIndex - 1}\n" +
			$"Before: {PeekAround(20, 0)}\n" +
			$"After: {PeekAround(0, 20)}";

		private string? PeekAround(int beforeIndex, int? afterIndex = null)
		{
			if (beforeIndex < 0) beforeIndex = -beforeIndex;
			if ((afterIndex ?? 0) < 0) afterIndex = -afterIndex;
			int start = NextIndex - beforeIndex;
			int end = NextIndex + (afterIndex ?? beforeIndex);
			if (start < 0) start = 0;
			if (end >= OrigString.Length) end = OrigString.Length - 1;
			return OrigString.Substring(start, end - start + 1);
		}

		public bool TryPopChar(out char c, bool consumeWhitespace)
		{
			c = '\0';
			do
			{
				if (NextIndex >= OrigString.Length) return false;
				c = OrigString[NextIndex];
				NextIndex++;
			} while (consumeWhitespace && char.IsWhiteSpace(c));
			return true;
		}

		public void Rewind()
		{
			NextIndex--;
		}
	}
}
