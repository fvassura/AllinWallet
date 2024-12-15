using AllinWallet.Models;
using AllinWallet.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace AllinWallet.ViewModels
{
    public class BaseConvertionListViewModel : ObservableObject
    {
        private readonly IStorageService _storageService;
        private TipoFile TipoFile = TipoFile.Csv;
        private string _baseFolderName;
        private string baseInputPath;
        private string baseOutputPath;
        private TipoFile csv;

        public ICommand ScegliFileCommand { get; }
        public ICommand ConvertiCommand { get; }


        public ObservableCollection<ConvertedFileViewModel> ConvertedFileList { get; }

        public BaseConvertionListViewModel(IStorageService storageService, TipoFile tipoFile, string baseFolderName)
        {
            _storageService = storageService;

            this.TipoFile = tipoFile;
            this._baseFolderName = baseFolderName;

            ScegliFileCommand = new Command(OnScegliFile);
            ConvertiCommand = new Command<ConvertedFileViewModel>(OnConverti);


            ConvertedFileList = new ObservableCollection<ConvertedFileViewModel>();

        }


        internal void OnPageAppearing()
        {
            LoadConvertedFileList();
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

            return true;
        }


        private void LoadConvertedFileList()
        {

            ConvertedFileList.Clear();
            //TODO: mettere il recupero da db sqlite
            /*     this.ConvertedFileList.Add(new ConvertedFileViewModel { DataImport = DateTime.Now, Nome = "Esempio di Import 1", Tipo = TipoFile.Csv, Elaborato = false, OutputFile = @"/storare/app/data/com.allinwallet/saty02.csv" });
                 this.ConvertedFileList.Add(new ConvertedFileViewModel { DataImport = DateTime.Now.AddDays(-1), Nome = "Esempio di Import 2", Tipo = TipoFile.Pdf, Elaborato = true, OutputFile = @"/storare/app/data/com.allinwallet/saty.db" });
                 this.ConvertedFileList.Add(new ConvertedFileViewModel { DataImport = DateTime.Now, Nome = "Esempio di Import 1", Tipo = TipoFile.Csv, Elaborato = false, OutputFile = @"/storare/app/data/com.allinwallet/saty02.csv" });
                 this.ConvertedFileList.Add(new ConvertedFileViewModel { DataImport = DateTime.Now.AddDays(-1), Nome = "Esempio di Import 2", Tipo = TipoFile.Pdf, Elaborato = true, OutputFile = @"/storare/app/data/com.allinwallet/saty.db" });
                 this.ConvertedFileList.Add(new ConvertedFileViewModel { DataImport = DateTime.Now, Nome = "Esempio di Import 1", Tipo = TipoFile.Csv, Elaborato = false, OutputFile = @"/storare/app/data/com.allinwallet/saty02.csv" });
                 this.ConvertedFileList.Add(new ConvertedFileViewModel { DataImport = DateTime.Now.AddDays(-1), Nome = "Esempio di Import 2", Tipo = TipoFile.Pdf, Elaborato = true, OutputFile = @"/storare/app/data/com.allinwallet/saty.db" });
                 this.ConvertedFileList.Add(new ConvertedFileViewModel { DataImport = DateTime.Now, Nome = "Esempio di Import 1", Tipo = TipoFile.Csv, Elaborato = false, OutputFile = @"/storare/app/data/com.allinwallet/saty02.csv" });
                 this.ConvertedFileList.Add(new ConvertedFileViewModel { DataImport = DateTime.Now.AddDays(-1), Nome = "Esempio di Import 2", Tipo = TipoFile.Pdf, Elaborato = true, OutputFile = @"/storare/app/data/com.allinwallet/saty.db" });
                 this.ConvertedFileList.Add(new ConvertedFileViewModel { DataImport = DateTime.Now, Nome = "Esempio di Import 1", Tipo = TipoFile.Csv, Elaborato = false, OutputFile = @"/storare/app/data/com.allinwallet/saty02.csv" });
                 this.ConvertedFileList.Add(new ConvertedFileViewModel { DataImport = DateTime.Now.AddDays(-1), Nome = "Esempio di Import 2", Tipo = TipoFile.Pdf, Elaborato = true, OutputFile = @"/storare/app/data/com.allinwallet/saty.db" });
                 this.ConvertedFileList.Add(new ConvertedFileViewModel { DataImport = DateTime.Now, Nome = "Esempio di Import 1", Tipo = TipoFile.Csv, Elaborato = false, OutputFile = @"/storare/app/data/com.allinwallet/saty02.csv" });
                 this.ConvertedFileList.Add(new ConvertedFileViewModel { DataImport = DateTime.Now.AddDays(-1), Nome = "Esempio di Import 2", Tipo = TipoFile.Pdf, Elaborato = true, OutputFile = @"/storare/app/data/com.allinwallet/saty.db" });

             */
        }

        private async void OnScegliFile()
        {
            try
            {
                // Selezione del file
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = GetCustomFileType(),
                    PickerTitle = $"Scegli un file {TipoFile}"
                });

                if (result != null)
                {

                    var workDir = this._storageService.CopyInputToWork(result, baseInputPath);


                    // Esegui azioni come la lettura del file, importazione, ecc.
                    await Application.Current.MainPage.DisplayAlert("File selezionato", $"File {result.FileName} importato in : {workDir} ", "OK");



                    /*
                    // Aggiungi lo storico del file scelto
                    ConvertedFileList.Add(new ConvertedFileViewModel
                    {
                        DataImport = DateTime.Now,
                        Nome = userFileName,
                        Tipo = userFileName.EndsWith(".csv") ? TipoFile.Csv : TipoFile.Pdf,
                        Elaborato = false,
                        OutputFile = userFilePath
                    });*/
                }
            }
            catch (Exception ex)
            {
                // Gestisci errori, ad esempio se non è stato scelto un file
                await Application.Current.MainPage.DisplayAlert("Errore", ex.Message, "OK");
            }

        }

        private FilePickerFileType GetCustomFileType()
        {
            FilePickerFileType result = FilePickerFileType.Jpeg;

            switch (TipoFile)
            {
                case TipoFile.Csv:
                    result = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                                    {
                                        { DevicePlatform.iOS, new[] { "public.comma-separated-values-text", "public.text" } }, // Aggiungi il tipo generico per CSV su iOS
                                        { DevicePlatform.Android, new[] { "text/csv", "application/vnd.ms-excel" } }, // Includi anche altri formati di file Excel
                                        { DevicePlatform.WinUI, new[] { ".csv", ".xls" } }, // Aggiungi anche Excel per Windows
                                        { DevicePlatform.macOS, new[] { "csv", "text/csv" } }, // Aggiungi supporto generico per CSV su macOS
                                    });
                    break;

                case TipoFile.Pdf:
                    result = FilePickerFileType.Pdf;
                    break;
            }

            return result;
        }


        private async void OnConverti(ConvertedFileViewModel item)
        {
            if (item != null)
            {
                await Application.Current.MainPage.DisplayAlert("Conversione", $"Hai selezionato: {item.Nome} #{this.ConvertedFileList.IndexOf(item)}", "OK");
            }
        }
    }



}
