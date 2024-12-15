using AllinWallet.Pages;
using AllinWallet.Services;
using AllinWallet.ViewModels;
using Microsoft.Extensions.Logging;

namespace AllinWallet
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialSymbolsOutlined-Regular.ttf", "MaterialSymbols");
                });

            builder.ConfigureFonts(fonts =>
            {
            });

            // Registrazione dei servizi

            builder.Services.AddSingleton<IStorageService, BaseStorageService>();

            builder.Services.AddSingleton<Satispay>();
            builder.Services.AddSingleton<SatispayViewModel>();

            builder.Services.AddSingleton<Nexi>();
            builder.Services.AddSingleton<NexiViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
