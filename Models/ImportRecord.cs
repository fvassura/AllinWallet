using System.ComponentModel.DataAnnotations;

namespace AllinWallet.Models
{

    public class ImportRecord
    {
        [Key]
        public int Id { get; set; }

        public string OriginPath { get; set; }
        public string OutputPath { get; set; }
        public DateTime ImportDate { get; set; }
    }
}
