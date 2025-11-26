using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VäderLabb;

namespace VäderLabb
{
    public class WeatherRecordMap : ClassMap<WeatherRecord>
    {
        public WeatherRecordMap()
        {
            Map(m => m.Datum).Name("Datum").TypeConverterOption.Format("yyyy-MM-dd H:mm");
            Map(m => m.Plats).Name("Plats");
            Map(m => m.Temp).Name("Temp").TypeConverter(new SafeDoubleConverter());
            Map(m => m.Luftfuktighet).Name("Luftfuktighet").TypeConverter(new SafeDoubleConverter());
        }
        public class SafeDoubleConverter : DefaultTypeConverter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                //Hantera null, tomma strängar och "N/A", t.ex. att returna null istället för 0
                if (string.IsNullOrWhiteSpace(text) ||
                    text.Equals("N/A", StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                //Försök konvertera direkt... text.Trim() för att ta bort eventuella mellanslag.
                if (double.TryParse(text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                {
                    return result;
                }

                return null;
            }
        }
    }
}
