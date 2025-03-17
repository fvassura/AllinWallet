using AllinWallet.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace AllinWallet.Services.Coverters
{
    public class SatispayConverter
    {

        public List<CsvSatispay> ReadCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    PrepareHeaderForMatch = args => args.Header.Replace(" ", "").ToLower(),
                    MissingFieldFound = null // Ignora i campi mancanti
                }))
                {
                    // Leggi i record come oggetti CsvSatispay
                    var righe = csv.GetRecords<CsvSatispay>().ToList();
                    return righe;
                }
            }
        }

        public void WriteCsvModified(string filePath, List<CsvSatispay> righe)
        {
            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    csv.WriteRecords(righe);
                }

            }
        }

    }
}
