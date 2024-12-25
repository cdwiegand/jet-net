using JetNet;

namespace JetTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        JsonParser p = new JsonParser("{ \"hello\": \"world\" }");
        JsonParseResult result = p.ProcessJson();

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(result[0] is JsonObject);
        JsonObject topLevelJo = result[0] as JsonObject ?? throw new InvalidCastException();
        Assert.IsNotNull(topLevelJo);
        Assert.AreEqual(1, topLevelJo.Items.Count);
        JsonProperty topLevelFirstChild = topLevelJo.Items[0] ?? throw new InvalidCastException();
        Assert.AreEqual("hello", topLevelFirstChild.Name);
        Assert.IsNotNull(topLevelFirstChild.Value);
        Assert.AreEqual(JsonValue.ValueTypes.String, topLevelFirstChild.Value.ValueType);
        JsonString topLevelFirstString = topLevelFirstChild.Value as JsonString ?? throw new InvalidCastException();
        Assert.IsNotNull(topLevelFirstString);
        Assert.AreEqual("world", topLevelFirstString.Value);
    }

    const string TestRootArrayJson = "[{ \"hello\": \"world\" }, { \"goodbye\": \"drama\", stay: [\"sunshine\", clou7ds, 'the `east\" wind', `water with spaces`], success: true }]";
    const string TestRootMultipleObjectsJson = "{ \"hello\": \"world\" }\n{ \"goodbye\": \"drama\", stay: [\"sunshine\", clou7ds, 'the `east\" wind', `water with spaces`], success: true }";
    const string CorrectRootArrayJson = "[ { \"hello\": \"world\" }, { \"goodbye\": \"drama\", \"stay\": [ \"sunshine\", \"clou7ds\", \"the `east\\\" wind\", \"water with spaces\" ], \"success\": true } ]";

    [TestMethod]
    public void TestRootArray()
    {
        JsonParser p = new JsonParser(TestRootArrayJson);
        JsonParseResult result = p.ProcessJson();

        Assert.IsNotNull(result);
        Assert.AreEqual(CorrectRootArrayJson, result.ToString());

        Assert.AreEqual(1, result.Count); // one array at root
        JsonArray jarr = result.First().AsArray();
        Assert.IsNotNull(jarr);

        TestHelper_Array(jarr);
    }

    [TestMethod]
    public void TestRootMultipleObjects()
    {
        JsonParser p = new JsonParser(TestRootMultipleObjectsJson);
        JsonParseResult result = p.ProcessJson();

        Assert.IsNotNull(result);
        Assert.AreEqual(CorrectRootArrayJson, result.ToString());

        TestHelper_Array(result);
    }

    private void TestHelper_Array(IList<JsonValue> jarr)
    {
        Assert.AreEqual(2, jarr.Count);

        JsonObject topLevelJo = jarr[0]!.AsObject();
        Assert.IsNotNull(topLevelJo);
        Assert.AreEqual(1, topLevelJo.Items.Count);
        JsonProperty topLevelFirstChild = topLevelJo.Items[0] ?? throw new InvalidCastException();
        Assert.AreEqual("hello", topLevelFirstChild.Name);
        Assert.IsNotNull(topLevelFirstChild.Value);
        Assert.AreEqual(JsonValue.ValueTypes.String, topLevelFirstChild.Value.ValueType);
        JsonString topLevelFirstString = topLevelFirstChild.Value as JsonString ?? throw new InvalidCastException();
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
        topLevelFirstString = topLevelFirstChild.Value as JsonString ?? throw new InvalidCastException();
        Assert.IsNotNull(topLevelFirstString);
        Assert.AreEqual("drama", topLevelFirstString.Value);
        topLevelFirstChild = topLevelJo.Items[1] ?? throw new InvalidCastException();
        Assert.AreEqual("stay", topLevelFirstChild.Name);
        Assert.IsNotNull(topLevelFirstChild.Value);
        Assert.AreEqual(JsonValue.ValueTypes.Array, topLevelFirstChild.Value.ValueType);
        JsonArray testMeArray = topLevelFirstChild.Value.AsArray();
        Assert.IsNotNull(testMeArray);
        Assert.AreEqual(4, testMeArray.Count);
        Assert.AreEqual("sunshine", testMeArray[0].AsString().Value);
        Assert.AreEqual("clou7ds", testMeArray[1].AsString().Value);
        Assert.AreEqual("the `east\" wind", testMeArray[2].AsString().Value);
        Assert.AreEqual("water with spaces", testMeArray[3].AsString().Value);
    }
}