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

    [TestMethod]
    public void TestRootArray()
    {
        JsonParser p = new JsonParser("[{ \"hello\": \"world\" }, { \"goodbye\": \"drama\", stay: [\"sunshine\", clouds, 'the `east\" wind', `water with spaces`], success: true }]");
        JsonParseResult result = p.ProcessJson();

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count); // one array at root
        JsonArray jarr = result.First().AsArray();
        Assert.IsNotNull(jarr);
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
        Assert.AreEqual(2, topLevelJo.Items.Count);
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
        Assert.AreEqual(JsonValue.ValueTypes.String, topLevelFirstChild.Value.ValueType);
        topLevelFirstString = topLevelFirstChild.Value as JsonString ?? throw new InvalidCastException();
        Assert.IsNotNull(topLevelFirstString);
        Assert.AreEqual("sunshine", topLevelFirstString.Value);
    }
}