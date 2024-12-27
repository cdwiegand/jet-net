using JetNet;

namespace JetTests;

[TestClass]
public class JsonPropertyTests
{
    [TestMethod]
    public void TestToString()
    {
        JsonFormatOptions format1 = JsonFormatOptions.Defaults.SetAlwaysQuoteRawNonNullValues();
        JsonFormatOptions format2 = JsonFormatOptions.Defaults.SetAddSpacesAroundDelimiters();
        JsonFormatOptions format3 = JsonFormatOptions.Defaults.SetAlwaysQuoteRawNonNullValues().SetAddSpacesAroundDelimiters();
        // 1 + 2 = 3.. haha!

        Assert.AreEqual("\"hello\":\"darkness my old friend\"", new JsonProperty("hello", "darkness my old friend").ToString());
        Assert.AreEqual("\"hello\":\"darkness my old friend\"", new JsonProperty("hello", "darkness my old friend").ToString(JsonFormatOptions.Defaults));
        Assert.AreEqual("\"hello\":\"darkness my old friend\"", new JsonProperty("hello", "darkness my old friend").ToString(format1));
        Assert.AreEqual("\"hello\": \"darkness my old friend\"", new JsonProperty("hello", "darkness my old friend").ToString(format2));
        Assert.AreEqual("\"hello\": \"darkness my old friend\"", new JsonProperty("hello", "darkness my old friend").ToString(format3));

        Assert.AreEqual("\"hello world\":null", new JsonProperty("hello world").ToString());
        Assert.AreEqual("\"hello world\":null", new JsonProperty("hello world").ToString(JsonFormatOptions.Defaults));
        Assert.AreEqual("\"hello world\":null", new JsonProperty("hello world").ToString(format1));
        Assert.AreEqual("\"hello world\": null", new JsonProperty("hello world").ToString(format2));
        Assert.AreEqual("\"hello world\": null", new JsonProperty("hello world").ToString(format3));

        Assert.AreEqual("\"hello world\":1", new JsonProperty("hello world", 1).ToString());
        Assert.AreEqual("\"hello world\":1", new JsonProperty("hello world", 1).ToString(JsonFormatOptions.Defaults));
        Assert.AreEqual("\"hello world\":\"1\"", new JsonProperty("hello world", 1).ToString(format1));
        Assert.AreEqual("\"hello world\": 1", new JsonProperty("hello world", 1).ToString(format2));
        Assert.AreEqual("\"hello world\": \"1\"", new JsonProperty("hello world", 1).ToString(format3));

        Assert.AreEqual("\"hello world\":true", new JsonProperty("hello world", true).ToString());
        Assert.AreEqual("\"hello world\":true", new JsonProperty("hello world", true).ToString(JsonFormatOptions.Defaults));
        Assert.AreEqual("\"hello world\":\"true\"", new JsonProperty("hello world", true).ToString(format1));
        Assert.AreEqual("\"hello world\": true", new JsonProperty("hello world", true).ToString(format2));
        Assert.AreEqual("\"hello world\": \"true\"", new JsonProperty("hello world", true).ToString(format3));

        Assert.AreEqual("\"hello \\\"world\\\"\":null", new JsonProperty("hello \"world\"").ToString());
        Assert.AreEqual("\"hello \\\"world\\\"\":null", new JsonProperty("hello \"world\"").ToString(JsonFormatOptions.Defaults));
        Assert.AreEqual("\"hello \\\"world\\\"\":null", new JsonProperty("hello \"world\"").ToString(format1));
        Assert.AreEqual("\"hello \\\"world\\\"\": null", new JsonProperty("hello \"world\"").ToString(format2));
        Assert.AreEqual("\"hello \\\"world\\\"\": null", new JsonProperty("hello \"world\"").ToString(format3));

        Assert.AreEqual("\"hello \\\"world\\\"\":true", new JsonProperty("hello \"world\"", true).ToString());
        Assert.AreEqual("\"hello \\\"world\\\"\":true", new JsonProperty("hello \"world\"", true).ToString(JsonFormatOptions.Defaults));
        Assert.AreEqual("\"hello \\\"world\\\"\":\"true\"", new JsonProperty("hello \"world\"", true).ToString(format1));
        Assert.AreEqual("\"hello \\\"world\\\"\": true", new JsonProperty("hello \"world\"", true).ToString(format2));
        Assert.AreEqual("\"hello \\\"world\\\"\": \"true\"", new JsonProperty("hello \"world\"", true).ToString(format3));
    }
}