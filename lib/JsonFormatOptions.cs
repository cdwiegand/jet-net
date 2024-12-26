using System;

namespace JetNet
{
	[Serializable]
	public class JsonFormatOptions : Exception
	{
		public bool AlwaysQuoteRawNonNullValues { get; set; }

		public bool AddSpacesAroundDelimiters { get; set; } // e.g. before and after [ ] { }   after (only) , :

		internal string FormatBeforeDelimiter(string value)
			=> (AddSpacesAroundDelimiters ? " " : "") + value;
		internal string FormatBeforeAfterDelimiter(string value)
			=> (AddSpacesAroundDelimiters ? " " : "") + value + (AddSpacesAroundDelimiters ? " " : "");
		internal string FormatAfterDelimiter(string value)
			=> value + (AddSpacesAroundDelimiters ? " " : "");

		public string EscapeJsonString(string? raw, bool allowRawNonNullValues)
		{
			if (allowRawNonNullValues && !AlwaysQuoteRawNonNullValues)
			{
				if (raw == null) return "null"; // json null value
				string rawL = raw.ToLower();
				if (rawL == "true") return "true"; // json boolean value
				if (rawL == "false") return "false"; // json boolean value
				if (decimal.TryParse(raw, out _)) return raw; // FIXME: json spec how format numbers?
			}
			if (raw == null) return "null"; // this always gets returned as-is, even when quoting "non-null" values
			return "\"" + (raw?.Replace("\\", "\\\\").Replace("\"", "\\\"") ?? "") + "\"";
		}

		public static JsonFormatOptions Defaults => new JsonFormatOptions();

		public JsonFormatOptions SetAlwaysQuoteRawNonNullValues(bool value = true)
		{
			AlwaysQuoteRawNonNullValues = value;
			return this;
		}
		public JsonFormatOptions SetAddSpacesAroundDelimiters(bool value = true)
		{
			AddSpacesAroundDelimiters = value;
			return this;
		}
	}
}
