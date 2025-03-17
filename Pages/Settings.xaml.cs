using AllinWallet.ViewModels;

namespace AllinWallet.Pages;

public partial class Settings : ContentPage
{
    private readonly SettingsViewModel _viewModel;

    public Settings(SettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Call any necessary methods in the ViewModel
        _viewModel.LoadSettingsAsync();
    }
}
