using JetNet;

namespace JetTests;

[TestClass]
public class JsonArrayTests
{
    [TestMethod]
    public void TestValueType()
    {
        Assert.AreEqual(JsonValue.ValueTypes.Array, new JsonArray().ValueType);
    }

    [TestMethod]
    public void TestToString()
    {
        JsonArray arr = new JsonArray();
        JsonObject obj = new JsonObject();
        obj.Add("hello", "world");
        obj.Add("spicy", true); // raw boolean values can be unquoted
        obj.Add("true", false); // keys must always be quoted
        arr.Add(obj);
        arr.Add(new JsonArray().AddMore("5", 3, null, 7.5, "7.8", true));
        Assert.AreEqual("[{}]", new JsonArray().AddMore(new JsonObject()).ToString());
        Assert.AreEqual("[{\"hello\":\"world\",\"spicy\":true,\"true\":false},[5,3,null,7.5,7.8,true]]", new JsonArray(arr).ToString());
    }
}