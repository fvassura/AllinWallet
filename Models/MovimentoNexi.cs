using CsvHelper.Configuration;

namespace AllinWallet.Models
{
    public class MovimentoNexi
    {
        public DateTime Data { get; set; }
        public string Descrizione { get; set; }
        public decimal Importo { get; set; }
    }

    public sealed class MovimentoMap : ClassMap<MovimentoNexi>
    {
        public MovimentoMap()
        {
            // Mappa per la proprietà Data con formattazione italiana
            Map(m => m.Data).TypeConverterOption.Format("dd/MM/yyyy");
            Map(m => m.Descrizione);
            Map(m => m.Importo);
        }
    }
}
