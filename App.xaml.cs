using AllinWallet.Models.Context;

namespace AllinWallet
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Inizializza il database all'avvio
            using (var db = new AllInWalletDbContext())
            {
                db.Initialize();
            }

            MainPage = new AppShell();
        }
    }
}
