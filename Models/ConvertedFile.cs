using SQLite;

namespace AllinWallet.Models
{

    [Table("ConvertedFiles")]
    public class ConvertedFile
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public TipoConversione TipoConversione { get; set; }

        public DateTime DataCreazione { get; set; }

        [MaxLength(255)]
        public string Nome { get; set; }

        public bool Convertito { get; set; }

        public DateTime DataConversione { get; set; }

        [MaxLength(512)]
        public string InputFile { get; set; }

        [MaxLength(512)]
        public string OutputFile { get; set; }
    }
}