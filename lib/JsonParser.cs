using System.Diagnostics;
using System.Text;

namespace JetNet
{
    public static class JsonParser
    {
        public static JsonParseResult ProcessJson(string json, Action<JsonObject>? topLevelHandler = null)
            => ProcessJson(new JsonStringReader(json), topLevelHandler);

        public static JsonParseResult ProcessJson(Stream stream, Action<JsonObject>? topLevelHandler = null)
            => ProcessJson(new JsonStreamReader(stream), topLevelHandler);

        public static JsonParseResult ProcessJson(StreamReader stream, Action<JsonObject>? topLevelHandler = null)
            => ProcessJson(new JsonStreamReaderReader(stream), topLevelHandler);

        public static JsonParseResult ProcessJson(StringReader stream, Action<JsonObject>? topLevelHandler = null)
            => ProcessJson(new JsonStringReaderReader(stream), topLevelHandler);

        public static JsonParseResult ProcessJson(IJsonReader StreamReader, Action<JsonObject>? topLevelHandler = null)
        {
            JsonParseResult ret = new JsonParseResult();
            while (StreamReader.TryPopChar(out char c, true))
            {
                if (c == '[')
                {
                    JsonArray current = ProcessJsonArray(StreamReader);
                    ret.Add(current);

                    foreach (var item in current.Items)
                        item.IfObject(itemObj => topLevelHandler?.Invoke(itemObj));
                }
                else if (c == '{')
                {
                    JsonObject current = ProcessJsonObject(StreamReader);
                    ret.Add(current);

                    topLevelHandler?.Invoke(current);
                }
                // else ignore it!
            }
            return ret;
        }

        public static JsonArray ProcessJsonArray(IJsonReader StreamReader)
        {
            JsonArray current = new();
            while (StreamReader.TryPopChar(out char c, true))
            {
                if (char.IsWhiteSpace(c) || c == ',')
                {
                    // ignore trailing/extraneous commas and whitespace
                }
                else if (c == '{')
                {
                    current.Add(ProcessJsonObject(StreamReader));
                }
                else if (c == '[')
                {
                    current.Add(ProcessJsonArray(StreamReader));
                }
                else if (c == ']')
                {
                    return current;
                }
                else if (c == '"' || c == '\'' || c == '`')
                {
                    string value = ReadQuotedString(StreamReader, c);
                    current.Add(new JsonStringValue(value));
                }
                else if (char.IsLetter(c)) // ok, letters mean a raw name... (sigh)
                {
                    StreamReader.Rewind();
                    string value = ReadUnquotedString(StreamReader);
                    current.Add(new JsonStringValue(value));
                }
                else
                {
                    throw new JetException($"Unknown char '{c}' found within array at index {StreamReader.CurrentIndex}");
                }
            }
            return current;
        }

        public static JsonObject ProcessJsonObject(IJsonReader StreamReader)
        {
            JsonObject current = new ();
            JsonProperty? currentAttr = null;

            while (StreamReader.TryPopChar(out char c, true))
            {
                if (c == '}') return current;

                if (currentAttr == null) // looking for a name (or end of our type)
                {
                    if (char.IsWhiteSpace(c) || c == ',') // ignore commas as well)
                    {
                        // ignore these
                    }
                    else if (c == '"' || c == '\'' || c == '`')
                    {
                        string attrName = ReadQuotedString(StreamReader, c);
                        currentAttr = new JsonProperty(attrName);
                    }
                    else if (char.IsLetterOrDigit(c))
                    {
                        StreamReader.Rewind();
                        string attrName = ReadUnquotedString(StreamReader);
                        currentAttr = new JsonProperty(attrName);
                    }
                    else
                        throw new JetException($"Unknown char '{c}' found within object at index {StreamReader.CurrentIndex}");
                    if (currentAttr?.Name == "index")
                        Debugger.Break();
                }
                else if (c == ':')
                {
                    // ignore name/value separator
                }
                else
                {
                    // ok, this set is different..
                    if (c == '"' || c == '\'' || c == '`')
                    {
                        string value = ReadQuotedString(StreamReader, c);
                        currentAttr.Value = new JsonStringValue(value);
                    }
                    else if (c == '{')
                    {
                        currentAttr.Value = ProcessJsonObject(StreamReader);
                    }
                    else if (c == '[')
                    {
                        currentAttr.Value = ProcessJsonArray(StreamReader);
                    }
                    else if (char.IsLetterOrDigit(c))
                    {
                        StreamReader.Rewind();
                        string value = ReadUnquotedString(StreamReader);
                        currentAttr.Value = JsonValue.BuildStringOrPrimitive(value);
                    }
                    current.Add(currentAttr);
                    currentAttr = null; // clear out
                }
            }

            return current;
        }

        public static string ReadQuotedString(IJsonReader StreamReader, char endingQuoteChar)
        {
            bool isEscaping = false;
            StringBuilder sb = new StringBuilder();
            while (StreamReader.TryPopChar(out char c, false))
            {
                if (c == '\\')
                    isEscaping = true;
                else if (isEscaping)
                    sb.Append(c);
                else if (c == endingQuoteChar)
                    break;
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        public static string ReadUnquotedString(IJsonReader StreamReader)
        {
            bool isEscaping = false;
            StringBuilder sb = new StringBuilder();
            while (StreamReader.TryPopChar(out char c, false))
            {
                if (c == '\\')
                    isEscaping = true;
                else if (isEscaping)
                {
                    sb.Append(c);
                    isEscaping = false;
                }
                else if (char.IsLetterOrDigit(c) || c == '.') // . permitted, even multiple times
                    sb.Append(c);
                else
                {
                    StreamReader.Rewind(); // "put the : back!"
                    break;
                }
            }
            return sb.ToString();
        }
    }
}
