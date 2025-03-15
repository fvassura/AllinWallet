using SQLite;

namespace AllinWallet.Services.SQLite
{
    public class SQLiteService
    {
        private readonly SQLiteAsyncConnection _database;

        public SQLiteService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "AllInWallet.db3");
            _database = new SQLiteAsyncConnection(dbPath);
        }

        // Metodo per ottenere la connessione al database (per il repository)
        public SQLiteAsyncConnection GetConnection() => _database;

        // Metodo per creare le tabelle
        public async Task CreateTableAsync<T>() where T : new()
        {
            await _database.CreateTableAsync<T>();
        }

    }
}
