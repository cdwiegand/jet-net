using JetNet;

namespace JetTests;

[TestClass]
public class MicrosoftEdgeGitHubJsonTests
{
	[TestMethod]
	public void TestUnterminated()
	{
		string json = @"{""name"": ""Preeti Rajdan"",""language"": ""Hindi"",""id"": ""3UN0X88Y4WYH3X8X"",""bio"": ""In sed ultricies lorem. Vivamus id faucibus velit, id posuere leo. Duis commodo orci ut dolor iaculis facilisis. Nam rutrum sollicitudin ante tempus consequat."",""version"": 9.17},{""name"": ""Sanjay Trivedi"",""language"": ""Hindi"",""id"": ""CPHR246457BD0""";
		var result = JsonParser.ProcessJson(json);
		Assert.IsNotNull(result);
		Assert.AreEqual(2, result.Count);

		JsonObject? first = result[0].AsObject();
		Assert.IsNotNull(first);
		Assert.AreEqual("Preeti Rajdan", first.AsString("name"));
		Assert.AreEqual("3UN0X88Y4WYH3X8X", first.AsString("id"));

		JsonObject? second = result[1].AsObject();
		Assert.IsNotNull(second);
		Assert.AreEqual("Sanjay Trivedi", second.AsString("name"));
		Assert.AreEqual("CPHR246457BD0", second.AsString("id"));
	}

	[TestMethod]
	public void TestMissingColon()
	{
		// from https://microsoftedge.github.io/Demos/json-dummy-data/missing-colon.json
		string json = @"{""name"": ""Noa Ervello"",""language"" ""Galician"",""id"": ""W9FR842CI16V8NU3"",""bio"": ""Aliquam sollicitudin ante ligula, eget malesuada nibh efficitur et. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Etiam consequat enim lorem, at tincidunt velit ultricies et. Suspendisse sit amet ullamcorper sem. Quisque mauris ligula, efficitur porttitor sodales ac, lacinia non ex."",""version"": 9.33}";
		// missing a colon between "language" and "Galician" ~^^^~ around here
		var result = JsonParser.ProcessJson(json);

		Assert.IsNotNull(result);
		Assert.AreEqual(1, result.Count);
		Assert.IsTrue(result[0].ValueType == JsonValue.ValueTypes.Object);
		JsonObject? root = result[0].AsObject();
		Assert.IsNotNull(root);
		Assert.AreEqual(5, root.Items.Count);
		Assert.IsNotNull(root["name"]);
		Assert.AreEqual("Noa Ervello", root["name"]?.AsString()?.Value);
		Assert.IsNotNull(root["language"]);
		Assert.AreEqual("Galician", root["language"]?.AsString()?.Value);
		Assert.IsNotNull(root["id"]);
		Assert.AreEqual("W9FR842CI16V8NU3", root["id"]?.AsString()?.Value);
		Assert.IsNotNull(root["bio"]);
		Assert.IsNotNull(root["version"]);
		Console.WriteLine(root["version"]?.ToString() ?? "is null");
		Assert.AreEqual(9.33m, root["version"]?.AsPrimitive<decimal>());
	}
}