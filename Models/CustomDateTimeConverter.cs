using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace AllinWallet.Models
{
    public class CustomDateTimeConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            // Definisci il formato della data atteso nel CSV
            string dateFormat = "dd MMM yyyy, HH:mm:ss"; // Formato del CSV
            var italianCulture = new CultureInfo("it-IT"); // Cultura italiana

            if (DateTime.TryParseExact(text, dateFormat, italianCulture, DateTimeStyles.None, out DateTime date))
            {
                return date; // Restituisci il valore come DateTime
            }
            return null;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            // Restituisci la data nel formato standard
            return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}