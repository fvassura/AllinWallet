using AllinWallet.Models;
using CsvHelper;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Globalization;

namespace AllinWallet.Services.Coverters
{
    public class NexiConverter
    {


        public List<MovimentoNexi> ReadPdf(string? pdfFile)
        {
            List<MovimentoNexi> movimentiNexi = new List<MovimentoNexi>();

            // Apri il PDF e leggi le pagine
            using (var pdfReader = new PdfReader(pdfFile))
            {
                using (var pdfDoc = new PdfDocument(pdfReader))
                {
                    // Estrai il testo dalla prima pagina
                    for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                    {
                        var page = pdfDoc.GetPage(i);
                        string pageText = PdfTextExtractor.GetTextFromPage(page);

                        // Trova la sezione "DETTAGLIO DEI SUOI MOVIMENTI"
                        if (pageText.Contains("DETTAGLIO DEI SUOI MOVIMENTI"))
                        {
                            // Dividi il testo per riga
                            var lines = pageText.Split("\n");

                            bool inDettMovimenti = false;
                            foreach (var line in lines)
                            {
                                if (line.Contains("DETTAGLIO DEI SUOI MOVIMENTI"))
                                {
                                    inDettMovimenti = true;
                                    continue;
                                }
                                if (inDettMovimenti && line.Contains("Data Descrizione Importo in Euro"))
                                {
                                    continue;
                                }

                                // Fine della tabella
                                if (line.Contains("TOTALE SPESE") || line.Contains("Per fare acquisti online con la tua carta è necessario"))
                                {
                                    inDettMovimenti = false;
                                    break;
                                }

                                if (inDettMovimenti)
                                {
                                    // Elabora la riga e aggiungila alla lista dei movimenti
                                    var movimentoNexi = ParseMovimento(line);
                                    if (movimentoNexi != null)
                                    {
                                        movimentiNexi.Add(movimentoNexi);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return movimentiNexi;
        }

        private MovimentoNexi ParseMovimento(string line)
        {
            try
            {
                // Pulizia della riga per separare i campi della tabella
                line = line.Trim();
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                // Esempio di formato: "31/08/23 Nextcharge Bologna 50,00"
                string data = parts[0]; // Data
                string descrizione = string.Join(" ", parts[1..^1]); // Descrizione fino all'ultimo campo
                string importoStr = parts[^1].Replace(".", "").Replace(",", "."); // Importo

                decimal importo = decimal.Parse(importoStr, CultureInfo.InvariantCulture);

                return new MovimentoNexi
                {
                    Data = DateTime.ParseExact(data, "dd/MM/yy", CultureInfo.InvariantCulture),
                    Descrizione = descrizione,
                    Importo = importo * -1 //va in negativo
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore nel parsing della linea: {line}. Dettagli: {ex.Message}");
                throw;
            }
        }
        public void WriteCsv(List<MovimentoNexi> movimentiNexi, string outputFileCsvPath)
        {
            // Scrivi i movimenti in un file CSV
            using (var writer = new StreamWriter(outputFileCsvPath))
            {
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {

                    csvWriter.Context.RegisterClassMap<MovimentoMap>();
                    csvWriter.WriteRecords(movimentiNexi);
                }
            }
        }
    }
}
