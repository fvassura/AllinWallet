using AllinWallet.Models;
using AllinWallet.Services;
using AllinWallet.Services.Coverters;
using AllinWallet.Services.SQLite;

namespace AllinWallet.ViewModels
{
    public class SatispayViewModel : BaseConvertionListViewModel
    {
        private SatispayConverter _converter;

        public SatispayViewModel(IStorageService storageService, SQLiteRepository<ConvertedFile> repo, SatispayConverter converter) :
            base(storageService, Models.TipoConversione.Csv, "Satispay", repo)
        {
            this._converter = converter;

        }

        protected override async Task ConvertiAction(ConvertedFileViewModel item)
        {
            var righeCsvSati = this._converter.ReadCsv(item.InputFile);
            this._converter.WriteCsvModified(item.OutputFile, righeCsvSati);

        }

        protected override string? GetWalletEmailTo(UserSettings settings)
        {
            return settings.EmailSatispay.Trim();
        }
    }



}
