using AllinWallet.ViewModels;

namespace AllinWallet.Pages;

public partial class Satispay : ContentPage
{
    public Satispay()
    {
        InitializeComponent();
        BindingContext = new SatispayViewModel();
    }
}