using JetNet;

namespace JetTests;

[TestClass]
public class JsonObjectTests
{
    [TestMethod]
    public void TestValueType()
    {
        Assert.AreEqual(JsonValue.ValueTypes.Object, new JsonObject().ValueType);
    }

    [TestMethod]
    public void TestItems() {
        JsonObject obj = new JsonObject();
        obj["valid"]= new JsonStringValue("yes");
        Assert.IsNull(obj["invalid"]);
        Assert.IsFalse(obj.ContainsKey("notvalid"));
        Assert.IsTrue(obj.ContainsKey("valid"));
        Assert.IsNotNull(obj["valid"]);
        Assert.AreEqual("\"yes\"", obj["valid"]?.ToString()); // yes, it should be a quoted string
        Assert.AreEqual("yes", obj["valid"]?.AsString()?.Value);
        Assert.AreEqual("yes", obj["valid"]?.AsString()?.Value?.ToString());
    }

    [TestMethod]
    public void TestToString()
    {
        Assert.AreEqual("{\"hello\":\"world\"}", new JsonObject().AddMore("hello", "world").ToString());
        Assert.AreEqual("{\"spicy\":true}", new JsonObject().AddMore("spicy", "True").ToString());
        Assert.AreEqual("{\"spicy\":1}", new JsonObject().AddMore("spicy", "1").ToString());
        Assert.AreEqual("{\"spicy\":1.0}", new JsonObject().AddMore("spicy", "1.0").ToString());
        Assert.AreEqual("{\"spicy\":\"1.0f\"}", new JsonObject().AddMore("spicy", "1.0f").ToString());
        Assert.AreEqual("{\"spicy\":\"null\"}", new JsonObject().AddMore("spicy", "null").ToString());
        Assert.AreEqual("{\"spicy\":null}", new JsonObject().AddMore("spicy", (object?)null).ToString());
    }
}