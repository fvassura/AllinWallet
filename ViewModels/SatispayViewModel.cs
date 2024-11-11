using AllinWallet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace AllinWallet.ViewModels
{
    public class SatispayViewModel : ObservableObject
    {
        private TipoFile TipoFile = TipoFile.Csv;

        public ICommand ChooseFileCommand { get; }
        public ICommand ItemTappedCommand { get; }
        public ObservableCollection<ImportedFileViewModel> ImportHistory { get; }

        public SatispayViewModel()
        {
            ChooseFileCommand = new Command(OnChooseFile);
            ItemTappedCommand = new Command<ImportedFileViewModel>(OnItemTapped);
            ImportHistory = new ObservableCollection<ImportedFileViewModel>()
                {
                    new ImportedFileViewModel { DataImport = DateTime.Now, Nome = "Esempio di Import 1", Tipo = TipoFile.Csv },
                    new ImportedFileViewModel { DataImport = DateTime.Now.AddDays(-1), Nome = "Esempio di Import 2", Tipo = TipoFile.Pdf }
                };
        }

        private async void OnChooseFile()
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
                    // Ottieni il percorso del file
                    string filePath = result.FullPath;
                    string fileName = result.FileName;

                    // Esegui azioni come la lettura del file, importazione, ecc.
                    await Application.Current.MainPage.DisplayAlert("File selezionato", $"File: {fileName}", "OK");

                    // Aggiungi lo storico del file scelto
                    ImportHistory.Add(new ImportedFileViewModel
                    {
                        DataImport = DateTime.Now,
                        Nome = fileName,
                        Tipo = fileName.EndsWith(".csv") ? TipoFile.Csv : TipoFile.Pdf,
                        Elaborato = false,
                        OutputFile = filePath
                    });
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

        private async void OnItemTapped(ImportedFileViewModel item)
        {
            if (item != null)
            {
                await Application.Current.MainPage.DisplayAlert("Elemento selezionato", $"Hai selezionato: {item.Nome}", "OK");
            }

        }
    }



}
