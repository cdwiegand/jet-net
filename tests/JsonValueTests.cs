using JetNet;

namespace JetTests;

[TestClass]
public class JsonValueTests
{
    [TestMethod]
    public void TestBasicJsonValue()
    {
        Assert.AreEqual("\"hello\"", JsonFormatOptions.Defaults.EscapeJsonString("hello", false));
        Assert.AreEqual("\"hello, world\"", JsonFormatOptions.Defaults.EscapeJsonString("hello, world", false));

        Assert.AreEqual("true", JsonFormatOptions.Defaults.EscapeJsonString("true", true));
        Assert.AreEqual("\"true\"", JsonFormatOptions.Defaults.EscapeJsonString("true", false));

        var format1 = JsonFormatOptions.Defaults.SetAlwaysQuoteRawNonNullValues();
        Assert.AreEqual("\"true\"", format1.EscapeJsonString("true", true));
        Assert.AreEqual("\"true\"", format1.EscapeJsonString("true", false));
    }

    [TestMethod]
    public void EscapeJsonString()
    {
        Assert.AreEqual("\"hello\"", JsonFormatOptions.Defaults.EscapeJsonString("hello", false));
        Assert.AreEqual("\"1\"", JsonFormatOptions.Defaults.EscapeJsonString("1", false));
        Assert.AreEqual("1", JsonFormatOptions.Defaults.EscapeJsonString("1", true));
        Assert.AreEqual("\"\"", JsonFormatOptions.Defaults.EscapeJsonString("", false));
        Assert.AreEqual("\"\"", JsonFormatOptions.Defaults.EscapeJsonString("", true));
        Assert.AreEqual("null", JsonFormatOptions.Defaults.EscapeJsonString(null, false));
        Assert.AreEqual("null", JsonFormatOptions.Defaults.EscapeJsonString(null, true));

        var format1 = JsonFormatOptions.Defaults.SetAlwaysQuoteRawNonNullValues();
        Assert.AreEqual("\"1\"", format1.EscapeJsonString("1", true));
        Assert.AreEqual("\"\"", format1.EscapeJsonString("", true));
        Assert.AreEqual("null", format1.EscapeJsonString(null, true));
    }

    [TestMethod]
    public void EscapeJsonString_WithSpaces()
    {
        Assert.AreEqual("\"hello, world\"", JsonFormatOptions.Defaults.EscapeJsonString("hello, world", false));
    }

    [TestMethod]
    public void EscapeJsonString_Booleans()
    {
        Assert.AreEqual("\"true\"", JsonFormatOptions.Defaults.EscapeJsonString("true", false));
        Assert.AreEqual("\"true\"", JsonFormatOptions.Defaults.SetAlwaysQuoteRawNonNullValues().EscapeJsonString("true", true));
        Assert.AreEqual("true", JsonFormatOptions.Defaults.EscapeJsonString("true", true));
        Assert.AreEqual("\"false\"", JsonFormatOptions.Defaults.EscapeJsonString("false", false));
        Assert.AreEqual("false", JsonFormatOptions.Defaults.EscapeJsonString("false", true));

        Assert.AreEqual("\"yes\"", JsonFormatOptions.Defaults.EscapeJsonString("yes", false));
        Assert.AreEqual("\"yes\"", JsonFormatOptions.Defaults.EscapeJsonString("yes", true));
        Assert.AreEqual("\"no\"", JsonFormatOptions.Defaults.EscapeJsonString("no", false));
        Assert.AreEqual("\"no\"", JsonFormatOptions.Defaults.EscapeJsonString("no", true));
    }

    [TestMethod]
    public void EscapeJsonString_Ints()
    {
        Assert.AreEqual("\"1\"", JsonFormatOptions.Defaults.EscapeJsonString("1", false));
        Assert.AreEqual("1", JsonFormatOptions.Defaults.EscapeJsonString("1", true));

        Assert.AreEqual("\" 1\"", JsonFormatOptions.Defaults.EscapeJsonString(" 1", false));
        Assert.AreEqual("\"1a\"", JsonFormatOptions.Defaults.EscapeJsonString("1a", false));
        Assert.AreEqual("\"1.\"", JsonFormatOptions.Defaults.EscapeJsonString("1.", false));
    }

    [TestMethod]
    public void EscapeJsonString_Decimals()
    {
        Assert.AreEqual("\"3.1415926\"", JsonFormatOptions.Defaults.EscapeJsonString("3.1415926", false));
        Assert.AreEqual("\"3.1415926\"", JsonFormatOptions.Defaults.SetAlwaysQuoteRawNonNullValues().EscapeJsonString("3.1415926", true));
        Assert.AreEqual("3.1415926", JsonFormatOptions.Defaults.EscapeJsonString("3.1415926", true));

        Assert.AreEqual("\" 3.1415926\"", JsonFormatOptions.Defaults.EscapeJsonString(" 3.1415926", false));
        Assert.AreEqual("\"3.1415926a\"", JsonFormatOptions.Defaults.EscapeJsonString("3.1415926a", false));
        Assert.AreEqual("\"3.1415926.\"", JsonFormatOptions.Defaults.EscapeJsonString("3.1415926.", false));
    }

    [TestMethod]
    public void CastAs()
    {
        Assert.ThrowsException<InvalidCastException>(() => new JsonArray().AsString());
        Assert.IsNotNull(new JsonArray().AsArray());
        Assert.ThrowsException<InvalidCastException>(() => new JsonArray().AsObject());
        Assert.ThrowsException<InvalidCastException>(() => new JsonObject().AsString());
        Assert.ThrowsException<InvalidCastException>(() => new JsonObject().AsArray());
        Assert.IsNotNull(new JsonObject().AsObject());
    }
}