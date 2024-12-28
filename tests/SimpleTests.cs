using JetNet;
using System.Text;

namespace JetTests;

[TestClass]
public class SimpleTests
{
    public const string HelloWorldJson = "{ \"hello\": \"world\" }";

    [TestMethod]
    public void TestHelloWorldString()
    {
        JsonParseResult result = JsonParser.ProcessJson(HelloWorldJson);

        Helper_ValidateHelloWorld(result);
    }

    [TestMethod]
    public void TestHelloWorldStringArray()
    {
        JsonParseResult result = JsonParser.ProcessJson("[" + HelloWorldJson + "," + HelloWorldJson + "]");

        Assert.IsNotNull(result);
        JsonArray? jarr = result[0].AsArray();

        Assert.IsNotNull(jarr);
        Assert.AreEqual(2, jarr.Count);
        foreach (var jarro in jarr)
        {
            JsonObject? jarroo = jarro.AsObject();
            Assert.IsNotNull(jarroo);
            Helper_ValidateHelloWorld(jarroo);
        }
    }

    [TestMethod]
    public void TestHelloWorldStringReader()
    {
        StringReader sr = new StringReader(HelloWorldJson);
        var result = JsonParser.ProcessJson(sr);

        Helper_ValidateHelloWorld(result);
    }

    [TestMethod]
    public void TestHelloWorldUtf8Stream()
    {
        MemoryStream ms = new MemoryStream();
        using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8, -1, true))
            sw.Write(HelloWorldJson);
        ms.Position = 0;

        var result = JsonParser.ProcessJson(ms);

        Helper_ValidateHelloWorld(result);
    }

    [TestMethod]
    public void TestHelloWorldUtf8StreamReader()
    {
        MemoryStream ms = new MemoryStream();
        using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8, -1, true))
            sw.Write(HelloWorldJson);
        ms.Position = 0;

        using (StreamReader sr = new StreamReader(ms))
        {
            var result = JsonParser.ProcessJson(ms);

            Helper_ValidateHelloWorld(result);
        }
    }

    [TestMethod]
    public void TestHelloWorldUnicodeStream()
    {
        MemoryStream ms = new MemoryStream();
        using (StreamWriter sw = new StreamWriter(ms, Encoding.Unicode, -1, true))
            sw.Write(HelloWorldJson);
        ms.Position = 0;

        var result = JsonParser.ProcessJson(ms);

        Helper_ValidateHelloWorld(result);
    }

    [TestMethod]
    public void TestHelloWorldUnicodeStreamReader()
    {
        MemoryStream ms = new MemoryStream();
        using (StreamWriter sw = new StreamWriter(ms, Encoding.Unicode, -1, true))
            sw.Write(HelloWorldJson);
        ms.Position = 0;

        using (StreamReader sr = new StreamReader(ms))
        {
            var result = JsonParser.ProcessJson(ms);

            Helper_ValidateHelloWorld(result);
        }
    }

    [TestMethod]
    public void TestXml()
    {
        string xml = "<not><really well=\"formed\">xml</really></not>"; // haha..
        JsonParseResult? result = JsonParser.ProcessJson(xml);
        Assert.AreEqual(0, result.Count);
    }

    public static void Helper_ValidateHelloWorld(JsonParseResult result)
    {
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(result[0] is JsonObject);
        JsonObject topLevelJo = result[0] as JsonObject ?? throw new InvalidCastException();
        Helper_ValidateHelloWorld(topLevelJo);
    }

    public static void Helper_ValidateHelloWorld(JsonObject topLevelJo)
    {
        Assert.IsNotNull(topLevelJo);
        Assert.AreEqual(1, topLevelJo.Items.Count);
        JsonProperty topLevelFirstChild = topLevelJo.Items[0] ?? throw new InvalidCastException();
        Assert.AreEqual("hello", topLevelFirstChild.Name);
        Assert.IsNotNull(topLevelFirstChild.Value);
        Assert.AreEqual(JsonValue.ValueTypes.String, topLevelFirstChild.Value.ValueType);
        JsonStringValue topLevelFirstString = topLevelFirstChild.Value as JsonStringValue ?? throw new InvalidCastException();
        Assert.IsNotNull(topLevelFirstString);
        Assert.AreEqual("world", topLevelFirstString.Value);
    }
}