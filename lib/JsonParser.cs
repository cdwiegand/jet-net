using System.Diagnostics;
using System.Text;

namespace JetNet
{
    public class JsonParser
    {
        public JsonParser(string json) : this(new JsonStringReader(json))
        {
        }

        public JsonParser(Stream stream) : this(new JsonStreamReader(stream))
        {
        }

        public JsonParser(StreamReader stream) : this(new JsonStreamReaderReader(stream))
        {
        }

        public JsonParser(StringReader stream) : this(new JsonStringReaderReader(stream))
        {
        }

        public JsonParser(IJsonReader streamReader)
        {
            StreamReader = streamReader;
        }

        public readonly IJsonReader StreamReader;

        public override string ToString() => StreamReader.Context;

        private List<Action<JsonObject>> TopLevelHandlers = new();

        public void AddHandler(Action<JsonObject> handler)
        {
            TopLevelHandlers.Add(handler);
        }

        public JsonParseResult ProcessJson()
        {
            JsonParseResult ret = new JsonParseResult();
            while (StreamReader.TryPopChar(out char c, true))
            {
                if (c == '[')
                {
                    JsonArray current = new JsonArray();
                    ret.Add(current);
                    ProcessJsonArray(current, true); // runs until the array is done
                }
                else if (c == '{')
                {
                    JsonObject current = new JsonObject();
                    ret.Add(current);
                    ProcessJsonObject(current, true); // runs until the object is done
                }
                // else ignore it!
            }
            return ret;
        }

        private void ProcessJsonArray(JsonArray current, bool isTopLevel)
        {
            while (StreamReader.TryPopChar(out char c, true))
            {
                if (char.IsWhiteSpace(c) || c == ',')
                {
                    // ignore trailing/extraneous commas and whitespace
                }
                else if (c == '{')
                {
                    JsonObject childO = new JsonObject();
                    current.Add(childO);
                    ProcessJsonObject(childO, isTopLevel); // if we are top-level array, then our objects are top-level objects (for handlers)
                }
                else if (c == '[')
                {
                    JsonArray childA = new JsonArray();
                    current.Add(childA);
                    ProcessJsonArray(childA, false);
                }
                else if (c == ']')
                {
                    return;
                }
                else if (c == '"' || c == '\'' || c == '`')
                {
                    string value = ParseString(c);
                    current.Add(new JsonStringValue(value));
                }
                else if (char.IsLetter(c)) // ok, letters mean a raw name... (sigh)
                {
                    StreamReader.Rewind();
                    string value = ParseUnquotedString();
                    current.Add(new JsonStringValue(value));
                }
                else
                {
                    throw new JetException($"Unknown char '{c}' found within array at index {StreamReader.CurrentIndex}");
                }
            }
        }

        private void ProcessJsonObject(JsonObject current, bool isTopLevel)
        {
            JsonProperty? currentAttr = null;

            while (StreamReader.TryPopChar(out char c, true))
            {
                if (c == '}')
                {
                    if (isTopLevel)
                        TopLevelHandlers?.ForEach(h => h(current));
                    return;
                }
                else if (currentAttr == null) // looking for a name (or end of our type)
                {
                    if (c == '"' || c == '\'' || c == '`')
                    {
                        string attrName = ParseString(c);
                        currentAttr = new JsonProperty(attrName);
                        current.Add(currentAttr);
                    }
                    else if (char.IsWhiteSpace(c) || c == ',') // ignore commas as well)
                    {
                        // ignore these
                    }
                    else if (char.IsLetterOrDigit(c))
                    {
                        StreamReader.Rewind();
                        string attrName = ParseUnquotedString();
                        currentAttr = new JsonProperty(attrName);
                        current.Add(currentAttr);
                    }
                    else
                        throw new JetException($"Unknown char '{c}' found within object at index {StreamReader.CurrentIndex}");
                    if (currentAttr?.Name == "success")
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
                        ProcessJsonValueString(currentAttr, c);
                    }
                    else if (c == '{')
                    {
                        ProcessJsonValueObject(currentAttr);
                    }
                    else if (c == '[')
                    {
                        ProcessJsonValueArray(currentAttr);
                    }
                    else if (char.IsLetterOrDigit(c))
                    {
                        StreamReader.Rewind();
                        string value = ParseUnquotedString();
                        currentAttr.Value = new JsonStringValue(value);
                    }
                    currentAttr = null; // clear out
                }
            }
        }

        private string ParseString(char endingQuoteChar)
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

        private string ParseUnquotedString()
        {
            bool isEscaping = false;
            StringBuilder sb = new StringBuilder();
            while (StreamReader.TryPopChar(out char c, false))
            {
                if (c == '\\')
                    isEscaping = true;
                else if (isEscaping)
                    sb.Append(c);
                else if (c == ':' || c == ',' || char.IsWhiteSpace(c))
                {
                    StreamReader.Rewind(); // "put the : back!"
                    break;
                }
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        private void ProcessJsonValueString(JsonProperty current, char endingQuoteChar)
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
            current.Value = new JsonStringValue(sb.ToString());
        }

        private void ProcessJsonValueArray(JsonProperty parent)
        {
            JsonArray current = new JsonArray();
            parent.Value = current;
            ProcessJsonArray(current, false);
        }

        private void ProcessJsonValueObject(JsonProperty parent)
        {
            JsonObject current = new JsonObject();
            parent.Value = current;
            ProcessJsonObject(current, false);
        }
    }
}
