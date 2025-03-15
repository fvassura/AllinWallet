using AllinWallet.Models;
using AllinWallet.Services;
using AllinWallet.Services.SQLite;

namespace AllinWallet.ViewModels
{
    public class SatispayViewModel : BaseConvertionListViewModel
    {

        public SatispayViewModel(IStorageService storageService, SQLiteRepository<ConvertedFile> repo) :
            base(storageService, Models.TipoConversione.Csv, "Satispay", repo)
        {

        }

    }



}
