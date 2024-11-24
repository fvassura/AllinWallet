using AllinWallet.ViewModels;

namespace AllinWallet.Pages;

public partial class Nexi : ContentPage
{
    public Nexi(NexiViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}