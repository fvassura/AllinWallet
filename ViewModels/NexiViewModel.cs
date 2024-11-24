using AllinWallet.Services;

namespace AllinWallet.ViewModels
{
    public class NexiViewModel : BaseConvertionListViewModel
    {
        public NexiViewModel(IStorageService storageService) :
            base(storageService, Models.TipoFile.Csv, "Nexi")
        {

        }
    }
}
