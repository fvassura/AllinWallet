using AllinWallet.ViewModels;

namespace AllinWallet.Pages;

public partial class Satispay : ContentPage
{
    private readonly SatispayViewModel _viewModel;
    public Satispay(SatispayViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Chiamata al metodo nel ViewModel
        _viewModel.OnPageAppearing();
    }

}