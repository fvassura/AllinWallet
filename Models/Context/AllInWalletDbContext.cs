
using Microsoft.EntityFrameworkCore;

namespace AllinWallet.Models.Context
{
    public class AllInWalletDbContext : DbContext
    {

        public DbSet<ImportRecord> ImportRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "AllInWallet.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");


            // Abilita il log per il debug
            //   optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }


        public void Initialize()
        {
            try
            {
                Database.Migrate();
            }
            catch (Exception ex)
            {
                // Gestisci l'errore (es. log, notifiche, ecc.)
                Console.WriteLine($"Errore durante l'aggiornamento del database: {ex.Message}");
            }
        }
    }
}
