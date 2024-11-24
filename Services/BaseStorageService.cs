
namespace AllinWallet.Services
{
    public class BaseStorageService : IStorageService
    {
        public virtual Task<bool> CheckAndRequestStoragePermissionAsync()
        {
            return Task.FromResult(true);
        }
        public virtual string GetStoragePath()
        {
            return "";
        }

        public string CopyInputToWork(FileResult result, string baseInputPath)
        {
            // Ottieni il percorso del file
            string userFilePath = result.FullPath;
            string userFileName = result.FileName;

            string workDir = Path.Combine(baseInputPath, userFileName);
            File.Copy(userFilePath, workDir, true);

            return workDir;
        }

    }
}