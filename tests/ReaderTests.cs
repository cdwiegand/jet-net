using JetNet;

namespace JetTests;

[TestClass]
public class ReaderTests
{
	[TestMethod]
	public void JsonStreamReader_MemoryStream_Runs()
	{
		MemoryStream ms = new MemoryStream();
		using (StreamWriter sw = new StreamWriter(ms, leaveOpen: true))
			sw.WriteLine(SimpleTests.HelloWorldJson);

		ms.Seek(0, SeekOrigin.Begin);    // reset
		var sr = new JsonStreamReader(ms);

		var result = JsonParser.ProcessJson(sr);
		SimpleTests.Helper_ValidateHelloWorld(result);
	}

	[TestMethod]
	public void JsonStreamReader_MemoryStream_Rewinds()
	{
		MemoryStream ms = new MemoryStream();
		using (StreamWriter sw = new StreamWriter(ms, leaveOpen: true))
			sw.Write("\"hello\"");

		ms.Seek(0, SeekOrigin.Begin);    // reset		
		var sr = new JsonStreamReader(ms);

		Helper_RewindTest(sr);
	}

	[TestMethod]
	public void JsonStreamReader_StreamReader_Rewinds()
	{
		MemoryStream ms = new MemoryStream();
		using (StreamWriter sw = new StreamWriter(ms, leaveOpen: true))
			sw.Write("\"hello\"");

		ms.Seek(0, SeekOrigin.Begin);    // reset		
		var sr = new JsonStreamReader(new StreamReader(ms));

		Helper_RewindTest(sr);
	}

	[TestMethod]
	public void JsonStringReader_String_Rewinds()
	{
		string s = "\"hello\"";

		JsonStringReader sr = new JsonStringReader(s);

		Helper_RewindTest(sr);
	}

	[TestMethod]
	public void JsonStringReader_StringReader_Rewinds()
	{
		string s = "\"hello\"";

		JsonStringReader sr = new JsonStringReader(new StringReader(s));

		Helper_RewindTest(sr);
	}

	private void Helper_RewindTest(IJsonReader sr)
	{
		char c;
		Assert.IsTrue(sr.TryPopChar(out c, true));
		Assert.AreEqual('\"', c);
		Assert.AreEqual("hello", JsonParser.ReadQuotedString(sr, '\"'));
		Assert.IsFalse(sr.TryPopChar(out c, true));
		sr.Rewind(); // put back the "
		Assert.IsTrue(sr.TryPopChar(out c, true) && c == '\"');
	}
}