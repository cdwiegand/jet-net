using System.Text;
using JetNet;

namespace JetTests;

[TestClass]
public class SimpleTests
{
    private const string HelloWorldJson = "{ \"hello\": \"world\" }";

    [TestMethod]
    public void TestHelloWorldString()
    {
        JsonParser p = new JsonParser(HelloWorldJson);
        JsonParseResult result = p.ProcessJson();

        Test_ValidateHelloWorld(result);
    }

    [TestMethod]
    public void TestHelloWorldStringReader()
    {
        StringReader sr = new StringReader(HelloWorldJson);
        JsonParser p = new JsonParser(sr);
        JsonParseResult result = p.ProcessJson();

        Test_ValidateHelloWorld(result);
    }

    [TestMethod]
    public void TestHelloWorldUtf8Stream()
    {
        MemoryStream ms = new MemoryStream();
        using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8, -1, true))
            sw.Write(HelloWorldJson);
        ms.Position = 0;

        JsonParser p = new JsonParser(ms);
        JsonParseResult result = p.ProcessJson();

        Test_ValidateHelloWorld(result);
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
            JsonParser p = new JsonParser(ms);
            JsonParseResult result = p.ProcessJson();

            Test_ValidateHelloWorld(result);
        }
    }

    [TestMethod]
    public void TestHelloWorldUnicodeStream()
    {
        MemoryStream ms = new MemoryStream();
        using (StreamWriter sw = new StreamWriter(ms, Encoding.Unicode, -1, true))
            sw.Write(HelloWorldJson);
        ms.Position = 0;

        JsonParser p = new JsonParser(ms);
        JsonParseResult result = p.ProcessJson();

        Test_ValidateHelloWorld(result);
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
            JsonParser p = new JsonParser(ms);
            JsonParseResult result = p.ProcessJson();

            Test_ValidateHelloWorld(result);
        }
    }

    private void Test_ValidateHelloWorld(JsonParseResult result)
    {
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
        JsonStringValue topLevelFirstString = topLevelFirstChild.Value as JsonStringValue ?? throw new InvalidCastException();
        Assert.IsNotNull(topLevelFirstString);
        Assert.AreEqual("world", topLevelFirstString.Value);
    }
}