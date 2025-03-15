using AllinWallet.Models;
using AllinWallet.Services;
using AllinWallet.Services.SQLite;

namespace AllinWallet.ViewModels
{
    public class NexiViewModel : BaseConvertionListViewModel
    {
        public NexiViewModel(IStorageService storageService, SQLiteRepository<ConvertedFile> repo) :
            base(storageService, Models.TipoConversione.Csv, "Nexi", repo)
        {

        }
    }
}
