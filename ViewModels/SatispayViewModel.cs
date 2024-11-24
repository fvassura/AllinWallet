using AllinWallet.Services;

namespace AllinWallet.ViewModels
{
    public class SatispayViewModel : BaseConvertionListViewModel
    {

        public SatispayViewModel(IStorageService storageService) :
            base(storageService, Models.TipoFile.Csv, "Satispay")
        {

        }

    }



}
