using AllinWallet.ViewModels;

namespace AllinWallet.Pages;

public partial class Satispay : ContentPage
{
    public Satispay(SatispayViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}