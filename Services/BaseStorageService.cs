
using AllinWallet.Models;
using System.Text.Json;

namespace AllinWallet.Services
{
    public class BaseStorageService : IStorageService
    {
        private const string SettingsFileName = "userSettings.json";
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


        public async Task SaveSettingsAsync(UserSettings settings)
        {
            string json = JsonSerializer.Serialize(settings);
            string filePath = Path.Combine(FileSystem.AppDataDirectory, SettingsFileName);
            await File.WriteAllTextAsync(filePath, json);
        }

        public async Task<UserSettings> LoadSettingsAsync()
        {
            string filePath = Path.Combine(FileSystem.AppDataDirectory, SettingsFileName);
            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<UserSettings>(json);
            }
            return new UserSettings();
        }

    }
}