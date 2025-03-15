
namespace AllinWallet.Services
{
    public interface IStorageService
    {
        Task<bool> CheckAndRequestStoragePermissionAsync();
        string CopyInputToWork(FileResult result, string baseInputPath);
        string GetStoragePath();

    }

}
