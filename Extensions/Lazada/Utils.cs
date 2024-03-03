using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using KTI.Moo.Extensions.Lazada.Model;

namespace KTI.Moo.Extensions.Lazada.Utils
{
    /// <summary>
    /// Converter to allow parsing the string "true" or any nonzero number as boolean true, and anything else as false.
    /// Needed for Lazada as all of their JSON values, including numbers and bools, are strings.
    /// </summary>
    public class BooleanJsonConverter : JsonConverter<Boolean>
    {
        public override Boolean Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False)
            {
                return reader.GetBoolean();
            }
            else
            {
                var value = reader.GetString();

                return !value.Equals("0") || value.ToLower().Equals("true");
            }
        }

        public override void Write(
            Utf8JsonWriter writer,
            Boolean booleanValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(booleanValue ? "true" : "false");
    }

    public class DateTimeJsonConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            DateTime output;
            string value = reader.GetString();

            if (reader.TokenType == JsonTokenType.Null || string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss zz00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out output))
            {
                return output;
            }
            else if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out output))
            {
                return output;
            }
            else if (DateTime.TryParseExact(value, "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out output))
            {
                return output;
            }

            return DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(value.Value.ToString("O"));
            else
                writer.WriteNullValue();
        }
    }

    public static class Extensions
    {
        public static IEnumerable<Category> Descendants(this Category root)
        {
            var categories = new Stack<Category>(new[] { root });
            while (categories.Any())
            {
                Category category = categories.Pop();
                yield return category;
                foreach (Category child in category.Children)
                    categories.Push(child);
            }
        }
    }
}
