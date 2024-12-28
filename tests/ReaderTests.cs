using JetNet;

namespace JetTests;

[TestClass]
public class ReaderTests
{
	[TestMethod]
	public void JsonStreamReaderTest_Runs()
	{
		MemoryStream ms = new MemoryStream();
		using (StreamWriter sw = new StreamWriter(ms, leaveOpen: true))
			sw.WriteLine(SimpleTests.HelloWorldJson);

		ms.Seek(0, SeekOrigin.Begin);    // reset
		JsonStreamReader sr = new JsonStreamReader(ms);
		var result = JsonParser.ProcessJson(sr);
		SimpleTests.Helper_ValidateHelloWorld(result);
	}

	[TestMethod]
	public void JsonStreamReaderTest_Rewinds()
	{
		MemoryStream ms = new MemoryStream();
		using (StreamWriter sw = new StreamWriter(ms, leaveOpen: true))
			sw.Write("\"hello\"");

		ms.Seek(0, SeekOrigin.Begin);    // reset		
		JsonStreamReader sr = new JsonStreamReader(ms);

		Helper_RewindTest(sr);
	}

	[TestMethod]
	public void JsonStreamReaderReaderTest_Rewinds()
	{
		MemoryStream ms = new MemoryStream();
		using (StreamWriter sw = new StreamWriter(ms, leaveOpen: true))
			sw.Write("\"hello\"");

		ms.Seek(0, SeekOrigin.Begin);    // reset		
		JsonStreamReaderReader sr = new JsonStreamReaderReader(new StreamReader(ms));

		Helper_RewindTest(sr);
	}

	[TestMethod]
	public void JsonStringReaderTest_Rewinds()
	{
		string s = "\"hello\"";

		JsonStringReader sr = new JsonStringReader(s);

		Helper_RewindTest(sr);
	}

	[TestMethod]
	public void JsonStringReaderReaderTest_Rewinds()
	{
		string s = "\"hello\"";

		JsonStringReaderReader sr = new JsonStringReaderReader(new StringReader(s));

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