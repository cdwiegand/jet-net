using JetNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetTests
{
    [TestClass]
    public class PartialJsonTests
    {
        private string FormatDate(int i) => (new DateTime(2020, 1, 1)).AddDays(i).ToString("yyyy-MM-dd");
        private string GenerateJsonFirstPart(int i) => $"{{ \"index\": {i}, \"date\": \"{FormatDate(i)}\",";
        private string GenerateJsonSecondPart(int i) => $"\"log\": \"This is line {i}.\" }}";
        private string GenerateJsonBothParts(int i) => GenerateJsonFirstPart(i) + GenerateJsonSecondPart(i);

        [TestMethod]
        public void TestPartialLineStringParsing()
        {
            MemoryStream ms = new MemoryStream();

            using StreamReader sr = new StreamReader(ms, Encoding.UTF8, leaveOpen: true);
            using StreamWriter sw = new StreamWriter(ms, Encoding.UTF8, leaveOpen: true);

            List<JsonObject> emittedObjects = new();

            JsonParser parser = new JsonParser(sr);
            parser.AddHandler(emittedObjects.Add);
            sw.Write(GenerateJsonFirstPart(1));
            sw.Flush();
            long oldPos = ms.Position;
            ms.Position = 0; // reset
            var result = parser.ProcessJson();
            ms.Position = oldPos; // push back
            Assert.IsTrue(emittedObjects.Count == 1); // but is partial

            // finish json object..
            sw.Write(GenerateJsonFirstPart(1));
            sw.WriteLine(GenerateJsonSecondPart(1));
            sw.Flush();
            ms.Position = oldPos; // reset back
            parser.ProcessJson();
            // don't care about ms.Position so not pushing back in
            Assert.IsTrue(emittedObjects.Count == 2);

            ValidateJson(emittedObjects[1], 1); // 2nd is valid
        }

        private void ValidateJson(JsonObject topLevelJo, int i)
        {
            Assert.IsNotNull(topLevelJo);
            Assert.AreEqual(3, topLevelJo.Items.Count);

            var theIndex = topLevelJo["index"] ?? throw new InvalidCastException();
            Assert.IsNotNull(theIndex);
            Assert.AreEqual(JsonValue.ValueTypes.Primitive, theIndex.ValueType);
            Assert.AreEqual(i, theIndex.AsPrimitive<int>());

            var theDate = topLevelJo["date"]?.AsString() ?? throw new InvalidCastException();
            Assert.IsNotNull(theDate);
            Assert.AreEqual(JsonValue.ValueTypes.String, theDate.ValueType);
            Assert.AreEqual(FormatDate(i), theDate.Value);

            var theLog = topLevelJo["log"]?.AsString() ?? throw new InvalidCastException();
            Assert.IsNotNull(theDate);
            Assert.AreEqual(JsonValue.ValueTypes.String, theLog.ValueType);
            Assert.AreEqual($"This is line {i}.", theLog.Value);
        }
    }
}
