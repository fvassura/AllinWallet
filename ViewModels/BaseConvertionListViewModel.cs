using AllinWallet.Models;
using AllinWallet.Services;
using AllinWallet.Services.SQLite;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace AllinWallet.ViewModels
{
    public abstract class BaseConvertionListViewModel : ObservableObject
    {
        protected readonly IStorageService _storageService;
        protected readonly SQLiteRepository<ConvertedFile> _convertedFileRepository;

        private TipoConversione TipoFile = TipoConversione.Csv;
        private string _baseFolderName;
        private string baseInputPath;
        private string baseOutputPath;
        private TipoConversione csv;

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand ScegliFileCommand { get; }
        public ICommand ConvertiCommand { get; }
        public ICommand DeleteCommand { get; }

        public ObservableCollection<ConvertedFileViewModel> ConvertedFileListVM { get; }
        public string TEST_EMAIL = "fvassu@gmail.com";

        public BaseConvertionListViewModel(IStorageService storageService, TipoConversione tipoFile, string baseFolderName, SQLiteRepository<ConvertedFile> repo)
        {
            _storageService = storageService;
            _convertedFileRepository = repo;

            this.TipoFile = tipoFile;
            this._baseFolderName = baseFolderName;
            Task.Run(async () => await InitializeStorageAsync());

            ScegliFileCommand = new Command(async () => await OnScegliFile());
            ConvertiCommand = new Command<ConvertedFileViewModel>(async (item) => await OnConverti(item));
            DeleteCommand = new Command<ConvertedFileViewModel>(async (item) => await OnDelete(item));


            ConvertedFileListVM = new ObservableCollection<ConvertedFileViewModel>();
        }

        internal async void OnPageAppearing()
        {
            await LoadConvertedFileList();
        }

        public async Task<bool> InitializeStorageAsync()
        {
            bool hasPermission = await _storageService.CheckAndRequestStoragePermissionAsync();
            if (!hasPermission)
            {
                return false;
            }

            string storagePath = _storageService.GetStoragePath();
            this.baseInputPath = Path.Combine(storagePath, _baseFolderName);
            this.baseOutputPath = Path.Combine(storagePath, _baseFolderName, "output");

            if (!Directory.Exists(baseInputPath))
            {
                Directory.CreateDirectory(baseInputPath);
            }

            if (!Directory.Exists(baseOutputPath))
            {
                Directory.CreateDirectory(baseOutputPath);
            }

            return true;
        }

        private async Task LoadConvertedFileList()
        {
            IsLoading = true;
            ConvertedFileListVM.Clear();

            var savedFiles = await _convertedFileRepository.GetAllAsync();
            foreach (var file in savedFiles)
            {
                ConvertedFileListVM.Add(new ConvertedFileViewModel(file));
            }

            IsLoading = false;
        }

        private async Task OnScegliFile()
        {
            try
            {
                // Selezione del file
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = null, //GetCustomFileType(),
                    PickerTitle = $"Scegli un file {TipoFile}"
                });

                if (result != null)
                {
                    var fileName = result.FileName;
                    var fileFullPathIn = _storageService.CopyInputToWork(result, baseInputPath);

                    // Esegui azioni come la lettura del file, importazione, ecc.
                    await Application.Current.MainPage.DisplayAlert("File selezionato", $"File {result.FileName} importato in : {fileFullPathIn} ", "OK");

                    var newVM = new ConvertedFileViewModel()
                    {
                        DataCreazione = DateTime.Now,
                        Nome = fileName,
                        InputFile = fileFullPathIn,
                        Tipo = this.TipoFile,
                        Convertito = false,
                        OutputFile = Path.Combine(baseOutputPath, fileName)
                    };

                    await _convertedFileRepository.SaveAsync(newVM.ToModel());

                    await LoadConvertedFileList();
                }
            }
            catch (Exception ex)
            {
                // Gestisci errori, ad esempio se non è stato scelto un file
                await Application.Current.MainPage.DisplayAlert("Errore", ex.Message, "OK");
            }
        }

        private async Task OnConverti(ConvertedFileViewModel item)
        {
            if (item != null)
            {
                await this.ConvertiAction(item);

                item.Convertito = true;
                item.DataConversione = DateTime.Now;
                await _convertedFileRepository.SaveAsync(item.ToModel());

                var invioOk = await Application.Current.MainPage.DisplayAlert("Conversione", $"Hai convertito: {item.Nome} #{ConvertedFileListVM.IndexOf(item)}, vuoi inviarlo via mail?", "OK", "Annulla");
                if (invioOk)
                {
                    var settings = await _storageService.LoadSettingsAsync();
                    var emailTo = this.GetWalletEmailTo(settings);
#if DEBUG
                    emailTo = this.TEST_EMAIL;
#endif
                    if (!string.IsNullOrWhiteSpace(emailTo))
                    {
                        // Invia il file via email a Wallet
                        var email = new EmailMessage
                        {
                            To = new List<string> { emailTo },
                            Subject = $"CSV convertito: {item.Nome}",
                            Body = $"Ecco il file convertito: {item.OutputFile}",
                            Attachments = new List<EmailAttachment> { new EmailAttachment(item.OutputFile) }
                        };
                        await Email.ComposeAsync(email);

                        await Application.Current.MainPage.DisplayAlert("Mail inviata", $"Inviato file: {item.OutputFile} per {this.TipoFile}", "OK");

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Conversione", $"Non hai impostato l'email per l'invio", "OK");
                        return;
                    }

                }

            }
        }

        protected abstract string? GetWalletEmailTo(UserSettings settings);

        protected abstract Task ConvertiAction(ConvertedFileViewModel item);

        private async Task OnDelete(ConvertedFileViewModel item)
        {
            if (item != null && ConvertedFileListVM.Contains(item))
            {
                ConvertedFileListVM.Remove(item);
                this._convertedFileRepository.DeleteAsync(item.Id);
            }
        }
    }



}
