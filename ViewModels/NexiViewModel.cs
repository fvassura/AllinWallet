using AllinWallet.Models;
using AllinWallet.Services;
using AllinWallet.Services.Coverters;
using AllinWallet.Services.SQLite;

namespace AllinWallet.ViewModels
{
    public class NexiViewModel : BaseConvertionListViewModel
    {
        private NexiConverter _converter;

        public NexiViewModel(IStorageService storageService, SQLiteRepository<ConvertedFile> repo, NexiConverter converter) :
            base(storageService, Models.TipoConversione.Csv, "Nexi", repo)
        {

            this._converter = converter;
        }

        protected override async Task ConvertiAction(ConvertedFileViewModel item)
        {
            // estraggo i movimenti dal pdf
            var movimentiNexi = _converter.ReadPdf(item.InputFile);

            if (movimentiNexi.Count > 0)
            {
                _converter.WriteCsv(movimentiNexi, item.OutputFile);

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Errore", $"Movimenti NON estratti da {item.InputFile}", "OK");
            }

        }

        protected override string? GetWalletEmailTo(UserSettings settings)
        {
            return settings.EmailNexi.Trim();
        }
    }
}
