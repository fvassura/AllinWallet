
using AllinWallet.Models;

namespace AllinWallet.Services
{
    public interface IStorageService
    {
        Task<bool> CheckAndRequestStoragePermissionAsync();
        string CopyInputToWork(FileResult result, string baseInputPath);
        string GetStoragePath();
        Task SaveSettingsAsync(UserSettings settings);
        Task<UserSettings> LoadSettingsAsync();
    }

}
