﻿using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Erbsenzaehler.Models;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Erbsenzaehler.Services.Importer
{
    public sealed class EasybankImporter : BaseImporter
    {
        public EasybankImporter(StreamReader reader)
            : base(reader)
        {
            Configuration.RegisterClassMap<LineMap>();
            Configuration.HasHeaderRecord = false;
            Configuration.Delimiter = ";";
            Configuration.Encoding = Encoding.Default;
        }

        public sealed class LineMap : CsvClassMap<Line>
        {
            public LineMap()
            {
                Map(x => x.Text).Index(1).TypeConverter<TextConverter>();
                Map(x => x.Date).Index(2).TypeConverter<DateConverter>();
                Map(x => x.OriginalDate).Index(2).TypeConverter<DateConverter>();
                Map(x => x.Amount).Index(4).TypeConverter<AmountConverter>();
            }

            public class AmountConverter : ITypeConverter
            {
                public string ConvertToString(TypeConverterOptions options, object value)
                {
                    return value.ToString();
                }

                public object ConvertFromString(TypeConverterOptions options, string text)
                {
                    return decimal.Parse(text, new CultureInfo("de-DE"));
                }

                public bool CanConvertFrom(Type type)
                {
                    return type == typeof(string);
                }

                public bool CanConvertTo(Type type)
                {
                    return type == typeof(string);
                }
            }

            public class DateConverter : ITypeConverter
            {
                public string ConvertToString(TypeConverterOptions options, object value)
                {
                    return ((DateTime)value).ToShortDateString();
                }

                public object ConvertFromString(TypeConverterOptions options, string text)
                {
                    return DateTime.Parse(text, new CultureInfo("de-DE"));
                }

                public bool CanConvertFrom(Type type)
                {
                    return type == typeof(string);
                }

                public bool CanConvertTo(Type type)
                {
                    return type == typeof(string);
                }
            }

            public class TextConverter : ITypeConverter
            {
                public string ConvertToString(TypeConverterOptions options, object value)
                {
                    return value as string;
                }

                public object ConvertFromString(TypeConverterOptions options, string text)
                {
                    return text.Replace("   ", " ").Replace("  ", " ").Replace("   ", " ").Replace("  ", " ").Trim();
                }

                public bool CanConvertFrom(Type type)
                {
                    return type == typeof(string);
                }

                public bool CanConvertTo(Type type)
                {
                    return type == typeof(string);
                }
            }
        }

    }
}