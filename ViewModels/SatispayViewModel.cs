using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace AllinWallet.ViewModels
{
    public class SatispayViewModel : ObservableObject
    {

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
            // Implementazione della selezione del file e della tua action post scelta
            await Application.Current.MainPage.DisplayAlert("File scelto", "Hai scelto un file!", "OK");

        }


        private async void OnItemTapped(ImportedFileViewModel item)
        {
            if (item != null)
            {
                await Application.Current.MainPage.DisplayAlert("Elemento selezionato", $"Hai selezionato: {item.ImportName}", "OK");
            }

        }
    }



}
