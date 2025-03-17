using AllinWallet.Models;
using AllinWallet.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AllinWallet.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        private readonly IStorageService _storageService;

        private string _emailNexi;
        public string EmailNexi
        {
            get => _emailNexi;
            set => SetProperty(ref _emailNexi, value);
        }

        private string _emailSatispay;
        public string EmailSatispay
        {
            get => _emailSatispay;
            set => SetProperty(ref _emailSatispay, value);
        }

        public IRelayCommand SaveSettingsCommand { get; }

        public SettingsViewModel(IStorageService storageService)
        {
            _storageService = storageService;
            SaveSettingsCommand = new RelayCommand(async () => await SaveSettingsAsync());
        }

        public async Task LoadSettingsAsync()
        {
            var settings = await _storageService.LoadSettingsAsync();
            EmailNexi = settings.EmailNexi;
            EmailSatispay = settings.EmailSatispay;
        }

        private async Task SaveSettingsAsync()
        {
            var settings = new UserSettings
            {
                EmailNexi = EmailNexi,
                EmailSatispay = EmailSatispay
            };
            await _storageService.SaveSettingsAsync(settings);
        }
    }
}

