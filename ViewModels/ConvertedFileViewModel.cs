using AllinWallet.Models;

namespace AllinWallet.ViewModels
{


    public class ConvertedFileViewModel
    {
        public TipoFile Tipo { get; set; }
        public DateTime DataImport { get; set; }
        public string Nome { get; set; }
        public bool Elaborato { get; set; }
        public string OutputFile { get; set; }

    }
}
