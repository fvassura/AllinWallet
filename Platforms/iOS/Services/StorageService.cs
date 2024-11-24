using AllinWallet.Services;

[assembly: Dependency(typeof(AllinWallet.Platforms.iOS.Services.StorageService))]
namespace AllinWallet.Platforms.iOS.Services
{
    public class StorageService : BaseStorageService
    {
        public override Task<bool> CheckAndRequestStoragePermissionAsync()
        {
            // iOS non richiede permessi espliciti per scrivere nella directory app
            return Task.FromResult(true);
        }



        public override string GetStoragePath()
        {
            return FileSystem.AppDataDirectory;
        }



    }

}
