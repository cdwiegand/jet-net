using JetNet;

namespace JetTests;

[TestClass]
public class JsonStringTests
{
    [TestMethod]
    public void TestValueType()
    {
		Assert.AreEqual(JsonValue.ValueTypes.String, new JsonStringValue("").ValueType);
	}

	[TestMethod]
	public void TestToString() {
		Assert.AreEqual("\"hello\"", new JsonStringValue("hello").ToString());
		Assert.AreEqual("\"hello\"", new JsonStringValue("hello").ToString(JsonFormatOptions.Defaults));
		Assert.AreEqual("\"hello world\"", new JsonStringValue("hello world").ToString());
		Assert.AreEqual("\"hello world\"", new JsonStringValue("hello world").ToString(JsonFormatOptions.Defaults));
		Assert.AreEqual("\"hello \\\"world\\\"\"", new JsonStringValue("hello \"world\"").ToString());
		Assert.AreEqual("\"hello \\\"world\\\"\"", new JsonStringValue("hello \"world\"").ToString(JsonFormatOptions.Defaults));
		Assert.AreEqual("true", new JsonStringValue("true").ToString());
		Assert.AreEqual("true", new JsonStringValue("true").ToString(JsonFormatOptions.Defaults));
		Assert.AreEqual("\"true\"", new JsonStringValue("true").ToString(JsonFormatOptions.Defaults.SetAlwaysQuoteRawNonNullValues()));
		Assert.AreEqual("true", new JsonStringValue("True").ToString());
		Assert.AreEqual("true", new JsonStringValue("True").ToString(JsonFormatOptions.Defaults));
		Assert.AreEqual("\"True\"", new JsonStringValue("True").ToString(JsonFormatOptions.Defaults.SetAlwaysQuoteRawNonNullValues()));
		Assert.AreEqual("\"truee\"", new JsonStringValue("truee").ToString());
		Assert.AreEqual("\"truee\"", new JsonStringValue("truee").ToString(JsonFormatOptions.Defaults));
	}
}