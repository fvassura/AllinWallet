using AllinWallet.Models;
using AllinWallet.Pages;
using AllinWallet.Services;
using AllinWallet.Services.SQLite;
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
            builder.Services.AddSingleton<IStorageService>(serviceProvider =>
            {
#if ANDROID
                    return new AllinWallet.Platforms.Android.Services.StorageService();
#elif IOS
                // Implementazione iOS se ne hai una
                return new AllinWallet.Platforms.iOS.Services.StorageService();
#else
                return new BaseStorageService();
#endif
            });


            builder.Services.AddSingleton<SQLiteService>();
            builder.Services.AddTransient<SQLiteRepository<ConvertedFile>>();


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
