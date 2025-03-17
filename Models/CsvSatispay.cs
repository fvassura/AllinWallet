using CsvHelper.Configuration.Attributes;

namespace AllinWallet.Models
{
    public class CsvSatispay
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Kind { get; set; }

        [TypeConverter(typeof(CustomDateTimeConverter))]
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string ExtraInfo { get; set; }
    }
}
