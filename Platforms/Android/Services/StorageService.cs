using AllinWallet.Services;
using Environment = Android.OS.Environment;

[assembly: Dependency(typeof(AllinWallet.Platforms.Android.Services.StorageService))]
namespace AllinWallet.Platforms.Android.Services
{
    public class StorageService : BaseStorageService
    {
        bool IsExternalStorageAvailable()
        {
            return Environment.ExternalStorageState == Environment.MediaMounted;
        }

        public override async Task<bool> CheckAndRequestStoragePermissionAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }
            return status == PermissionStatus.Granted;
        }


        public override string GetStoragePath()
        {
            // Controlla se la memoria esterna è montata
            if (IsExternalStorageAvailable())
            {
                return Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDocuments).AbsolutePath;
            }
            else
            {
                // Usa la directory dati dell'app come fallback
                return FileSystem.AppDataDirectory;
            }
        }


    }

}
