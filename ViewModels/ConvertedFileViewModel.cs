using AllinWallet.Models;

namespace AllinWallet.ViewModels
{


    public class ConvertedFileViewModel
    {

        public TipoConversione Tipo { get; set; }
        public DateTime DataCreazione { get; set; }
        public string Nome { get; set; }
        public bool Convertito { get; set; }
        public DateTime DataConversione { get; set; }
        public string InputFile { get; set; }
        public string OutputFile { get; set; }
        public int Id { get; internal set; }

        public ConvertedFileViewModel()
        {
        }
        public ConvertedFileViewModel(ConvertedFile model)
        {
            this.Id = model.Id;
            this.Tipo = model.TipoConversione;
            this.DataCreazione = model.DataCreazione;
            this.Nome = model.Nome;
            this.Convertito = model.Convertito;
            this.DataConversione = model.DataConversione;
            this.InputFile = model.InputFile;
            this.OutputFile = model.OutputFile;
        }

        public ConvertedFile ToModel()
        {
            return new ConvertedFile
            {
                Id = this.Id,
                TipoConversione = this.Tipo,
                DataCreazione = this.DataCreazione,
                Nome = this.Nome,
                Convertito = this.Convertito,
                DataConversione = this.DataConversione,
                InputFile = this.InputFile,
                OutputFile = this.OutputFile
            };
        }



    }
}
