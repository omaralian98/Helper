using SQLite;
using Helper.Models;

namespace Helper.Data;

public class HelperDatabase<T> where T : Entity, new()
{
    protected SQLiteAsyncConnection Database;

    public HelperDatabase()
    {
        //DropTables();
        CreateTables().ConfigureAwait(false);
    }

    private async Task CreateTables()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await Database.CreateTableAsync<T>();
    }

    private async Task DropTables()
    {
        if (Database is not null)
            return;

        try
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await Database.DropTableAsync<T>();
        }
        catch { }
    }

    protected async Task<List<T>> GetAllAsync()
    {
        return await Database.Table<T>().ToListAsync();
    }

    protected async Task<T> GetAsync(int id)
    {
        return await Database.Table<T>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    protected async Task<int> SaveAsync(T operation)
    {
        if (operation.Id != 0)
            return await Database.UpdateAsync(operation);
        else
            return await Database.InsertAsync(operation);
    }

    protected async Task<int> DeleteAsync(T operation)
    {
        return await Database.DeleteAsync(operation);
    }
}
