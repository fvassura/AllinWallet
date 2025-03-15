using SQLite;

namespace AllinWallet.Services.SQLite
{
    public class SQLiteRepository<T> where T : new()
    {
        private readonly SQLiteAsyncConnection _database;

        public SQLiteRepository(SQLiteService svc)
        {
            _database = svc.GetConnection();
            _database.CreateTableAsync<T>().Wait();
        }

        // Carica tutti i record
        public async Task<List<T>> GetAllAsync()
        {
            return await _database.Table<T>().ToListAsync();
        }

        // Carica un record per ID
        public async Task<T> GetByIdAsync(int id)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
            {
                throw new InvalidOperationException("Type does not contain a property named 'Id'");
            }

            return await _database.Table<T>().Where(x => (int)idProperty.GetValue(x) == id).FirstOrDefaultAsync();
        }

        // Salva o aggiorna un record
        public async Task SaveAsync(T entity)
        {
            var idProperty = typeof(T).GetProperty("Id");
            var idValue = (int)(idProperty?.GetValue(entity) ?? 0);

            if (idValue == 0)
            {
                // Nuovo record
                await _database.InsertAsync(entity);
            }
            else
            {
                // Aggiornamento
                await _database.UpdateAsync(entity);
            }
        }

        // Elimina un record
        public async Task DeleteAsync(int id)
        {
            await _database.DeleteAsync<T>(id);
        }
    }

}
