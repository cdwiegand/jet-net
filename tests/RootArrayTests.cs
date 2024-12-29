using JetNet;

namespace JetTests;

[TestClass]
public class RootArrayTests
{
    public const string TestRootArrayJson = "[{ \"hello\": \"world\" }, { \"goodbye\": \"drama\", stay: [\"sunshine\", clou7ds, 'the `east\" wind', `water with spaces`], success: true }]";
    public const string TestRootMultipleObjectsJson = "{ \"hello\": \"world\" }\n{ \"goodbye\": \"drama\", stay: [\"sunshine\", clou7ds, 'the `east\" wind', `water with spaces`], success: true }";
    public const string CorrectRootArrayJson_Defaults = "[{\"hello\":\"world\"},{\"goodbye\":\"drama\",\"stay\":[\"sunshine\",\"clou7ds\",\"the `east\\\" wind\",\"water with spaces\"],\"success\":true}]";
    public const string CorrectRootArrayJson_QuoteRaw = "[{\"hello\":\"world\"},{\"goodbye\":\"drama\",\"stay\":[\"sunshine\",\"clou7ds\",\"the `east\\\" wind\",\"water with spaces\"],\"success\":\"true\"}]";
    public const string CorrectRootArrayJson_DelimSpace = "[ { \"hello\": \"world\" }, { \"goodbye\": \"drama\", \"stay\": [ \"sunshine\", \"clou7ds\", \"the `east\\\" wind\", \"water with spaces\" ], \"success\": true } ]";
    public const string CorrectRootArrayJson_QuoteRaw_DelimSpace = "[ { \"hello\": \"world\" }, { \"goodbye\": \"drama\", \"stay\": [ \"sunshine\", \"clou7ds\", \"the `east\\\" wind\", \"water with spaces\" ], \"success\": \"true\" } ]";

    [TestMethod]
    public void TestRootArray()
    {
        var result = JsonParser.ProcessJson(TestRootArrayJson);

        Assert.IsNotNull(result);
        Assert.AreEqual(CorrectRootArrayJson_Defaults, result.ToString());
        Assert.AreEqual(CorrectRootArrayJson_Defaults, result.ToString(JsonFormatOptions.Defaults));
        Assert.AreEqual(CorrectRootArrayJson_QuoteRaw, result.ToString(JsonFormatOptions.Defaults.SetAlwaysQuoteRawNonNullValues()));
        Assert.AreEqual(CorrectRootArrayJson_DelimSpace, result.ToString(JsonFormatOptions.Defaults.SetAddSpacesAroundDelimiters()));
        Assert.AreEqual(CorrectRootArrayJson_QuoteRaw_DelimSpace, result.ToString(JsonFormatOptions.Defaults.SetAlwaysQuoteRawNonNullValues().SetAddSpacesAroundDelimiters()));

        Assert.AreEqual(1, result.Count); // one array at root
        JsonArray? jarr = result.First().AsArray();
        Assert.IsNotNull(jarr);

        Helper_Array(jarr);

        // now, render back to json, then re-parse, and re-validate
        string json = result.ToString();
        result = JsonParser.ProcessJson(json);

        jarr = result.First().AsArray();
        Assert.IsNotNull(jarr);
        Helper_Array(jarr);
        Assert.AreEqual(json, result.ToString());
    }

    [TestMethod]
    public void TestRootMultipleObjects()
    {
        var result = JsonParser.ProcessJson(TestRootMultipleObjectsJson);

        Assert.IsNotNull(result);
        Assert.AreEqual(CorrectRootArrayJson_Defaults, result.ToString());
        Assert.AreEqual(CorrectRootArrayJson_Defaults, result.ToString(JsonFormatOptions.Defaults));
        Assert.AreEqual(CorrectRootArrayJson_QuoteRaw, result.ToString(JsonFormatOptions.Defaults.SetAlwaysQuoteRawNonNullValues()));
        Assert.AreEqual(CorrectRootArrayJson_DelimSpace, result.ToString(JsonFormatOptions.Defaults.SetAddSpacesAroundDelimiters()));
        Assert.AreEqual(CorrectRootArrayJson_QuoteRaw_DelimSpace, result.ToString(JsonFormatOptions.Defaults.SetAlwaysQuoteRawNonNullValues().SetAddSpacesAroundDelimiters()));

        Helper_Array(result);

        // now, render back to json, then re-parse, and re-validate
        string json = result.ToString();
        result = JsonParser.ProcessJson(json);

        Assert.IsNotNull(result);
        Helper_Array(result.First().AsArray()); 
        // we test differently here because the original was two newline-separated objects, 
        // but the result is an array (normalizing it)
        Assert.AreEqual(json, result.ToString());
    }

    private void Helper_Array(IList<JsonValue>? jarr)
    {
        Assert.IsNotNull(jarr);
        Assert.AreEqual(2, jarr.Count);

        JsonObject? topLevelJo = jarr[0].AsObject();
        Assert.IsNotNull(topLevelJo);
        Assert.AreEqual(1, topLevelJo.Items.Count);
        JsonProperty topLevelFirstChild = topLevelJo.Items[0] ?? throw new InvalidCastException();
        Assert.AreEqual("hello", topLevelFirstChild.Name);
        Assert.IsNotNull(topLevelFirstChild.Value);
        Assert.AreEqual(JsonValue.ValueTypes.String, topLevelFirstChild.Value.ValueType);
        JsonStringValue topLevelFirstString = topLevelFirstChild.Value as JsonStringValue ?? throw new InvalidCastException();
        Assert.IsNotNull(topLevelFirstString);
        Assert.AreEqual("world", topLevelFirstString.Value);

        topLevelJo = jarr[1]!.AsObject();
        Assert.IsNotNull(topLevelJo);
        Assert.AreEqual(3, topLevelJo.Items.Count);
        Assert.AreEqual("goodbye", topLevelJo.Items[0].Name);
        Assert.AreEqual("stay", topLevelJo.Items[1].Name);
        Assert.AreEqual("success", topLevelJo.Items[2].Name);

        topLevelFirstChild = topLevelJo.Items[0] ?? throw new InvalidCastException();
        Assert.AreEqual("goodbye", topLevelFirstChild.Name);
        Assert.IsNotNull(topLevelFirstChild.Value);
        Assert.AreEqual(JsonValue.ValueTypes.String, topLevelFirstChild.Value.ValueType);
        topLevelFirstString = topLevelFirstChild.Value as JsonStringValue ?? throw new InvalidCastException();
        Assert.IsNotNull(topLevelFirstString);
        Assert.AreEqual("drama", topLevelFirstString.Value);
        topLevelFirstChild = topLevelJo.Items[1] ?? throw new InvalidCastException();
        Assert.AreEqual("stay", topLevelFirstChild.Name);
        Assert.IsNotNull(topLevelFirstChild.Value);
        Assert.AreEqual(JsonValue.ValueTypes.Array, topLevelFirstChild.Value.ValueType);
        JsonArray? testMeArray = topLevelFirstChild.Value.AsArray();
        Assert.IsNotNull(testMeArray);
        Assert.AreEqual(4, testMeArray.Count);
        Assert.AreEqual("sunshine", testMeArray[0]?.AsString()?.Value ?? "");
        Assert.AreEqual("clou7ds", testMeArray[1]?.AsString()?.Value ?? "");
        Assert.AreEqual("the `east\" wind", testMeArray[2]?.AsString()?.Value ?? "");
        Assert.AreEqual("water with spaces", testMeArray[3]?.AsString()?.Value ?? "");
    }
}