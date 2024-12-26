using System;

namespace JetNet
{
	public interface IJsonReader
	{
		int NextIndex { get; }
		int CurrentIndex { get; }
		string Context { get; }
		bool TryPopChar(out char c, bool consumeWhitespace);
		void Rewind();
	}
}