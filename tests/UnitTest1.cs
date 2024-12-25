using JetNet;

namespace JetTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        JsonParser p = new JsonParser("{ \"hello\": \"world\" }");
        JsonArray? result = p.ProcessJson();

        Assert.IsNotNull(result);
        Assert.IsTrue(result is JsonArray);
        Assert.AreEqual(1, result.Children.Count);
        Assert.IsTrue(result.Children[0] is JsonObject);
        JsonObject topLevelJo = result.Children[0] as JsonObject ?? throw new InvalidCastException();
        Assert.IsNotNull(topLevelJo);
        Assert.AreEqual(1, topLevelJo.Children.Count);
        JsonAttribute topLevelFirstChild = topLevelJo.Children[0] ?? throw new InvalidCastException();
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
        JsonArray? result = p.ProcessJson();

        Assert.IsNotNull(result);
        Assert.IsTrue(result is JsonArray);
        Assert.AreEqual(2, result.Children.Count);

        Assert.IsTrue(result.Children[0] is JsonObject);
        JsonObject topLevelJo = result.Children[0] as JsonObject ?? throw new InvalidCastException();
        Assert.IsNotNull(topLevelJo);
        Assert.AreEqual(1, topLevelJo.Children.Count);
        JsonAttribute topLevelFirstChild = topLevelJo.Children[0] ?? throw new InvalidCastException();
        Assert.AreEqual("hello", topLevelFirstChild.Name);
        Assert.IsNotNull(topLevelFirstChild.Value);
        Assert.AreEqual(JsonValue.ValueTypes.String, topLevelFirstChild.Value.ValueType);
        JsonString topLevelFirstString = topLevelFirstChild.Value as JsonString ?? throw new InvalidCastException();
        Assert.IsNotNull(topLevelFirstString);
        Assert.AreEqual("world", topLevelFirstString.Value);

        Assert.IsTrue(result.Children[1] is JsonObject);
        topLevelJo = result.Children[1] as JsonObject ?? throw new InvalidCastException();
        Assert.IsNotNull(topLevelJo);
        Assert.AreEqual(2, topLevelJo.Children.Count);
        topLevelFirstChild = topLevelJo.Children[0] ?? throw new InvalidCastException();
        Assert.AreEqual("goodbye", topLevelFirstChild.Name);
        Assert.IsNotNull(topLevelFirstChild.Value);
        Assert.AreEqual(JsonValue.ValueTypes.String, topLevelFirstChild.Value.ValueType);
        topLevelFirstString = topLevelFirstChild.Value as JsonString ?? throw new InvalidCastException();
        Assert.IsNotNull(topLevelFirstString);
        Assert.AreEqual("drama", topLevelFirstString.Value);
        topLevelFirstChild = topLevelJo.Children[1] ?? throw new InvalidCastException();
        Assert.AreEqual("stay", topLevelFirstChild.Name);
        Assert.IsNotNull(topLevelFirstChild.Value);
        Assert.AreEqual(JsonValue.ValueTypes.String, topLevelFirstChild.Value.ValueType);
        topLevelFirstString = topLevelFirstChild.Value as JsonString ?? throw new InvalidCastException();
        Assert.IsNotNull(topLevelFirstString);
        Assert.AreEqual("sunshine", topLevelFirstString.Value);
    }
}