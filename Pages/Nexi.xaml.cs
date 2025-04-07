using AllinWallet.ViewModels;

namespace AllinWallet.Pages;

public partial class Nexi : ContentPage
{
    private readonly NexiViewModel _viewModel;
    public Nexi(NexiViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Chiamata al metodo nel ViewModel
        _viewModel.OnPageAppearing();
    }

}