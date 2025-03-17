using AllinWallet.Models;
using AllinWallet.Services;
using AllinWallet.Services.SQLite;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace AllinWallet.ViewModels
{
    public class BaseConvertionListViewModel : ObservableObject
    {
        private readonly IStorageService _storageService;
        private readonly SQLiteRepository<ConvertedFile> _convertedFileRepository;

        private TipoConversione TipoFile = TipoConversione.Csv;
        private string _baseFolderName;
        private string baseInputPath;
        private string baseOutputPath;
        private TipoConversione csv;

        public ICommand ScegliFileCommand { get; }
        public ICommand ConvertiCommand { get; }
        public ICommand DeleteCommand { get; }

        public ObservableCollection<ConvertedFileViewModel> ConvertedFileListVM { get; }

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
            ConvertedFileListVM.Clear();

            var savedFiles = await _convertedFileRepository.GetAllAsync();
            foreach (var file in savedFiles)
            {
                ConvertedFileListVM.Add(new ConvertedFileViewModel(file));
            }
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
                await Application.Current.MainPage.DisplayAlert("Conversione", $"Hai selezionato: {item.Nome} #{ConvertedFileListVM.IndexOf(item)}", "OK");
            }
        }

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
