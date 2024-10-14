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

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await Database.Table<T>().ToListAsync();
    }

    public virtual async Task<T?> GetAsync(int? id = null)
    {
        var test = await Database.Table<T>().ToListAsync();
        if (id is not null)
        {
            return await Database.Table<T>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }
        else
        {
            return await Database.Table<T>().FirstOrDefaultAsync();
        }
    }

    public virtual async Task<int> AddAsync(T operation)
    {
        try
        {
            return await Database.InsertAsync(operation);

        }
        catch
        {
            return 0;
        }
    }

    public async Task<int> DeleteAsync(T operation)
    {
        return await Database.DeleteAsync(operation);
    }

    public virtual async Task<int> GetFirst() => await GetFirstOrLast(last: false);

    public virtual async Task<int> GetLast() => await GetFirstOrLast(last: true);

    private async Task<int> GetFirstOrLast(bool last)
    {
        string IsDESC = last ? "DESC" : "";

        var query = $@"
            SELECT 
                [{nameof(Entity.DateAsString)}]
            FROM 
                [{typeof(T).Name}]
            ORDER BY [{nameof(Entity.DateAsString)}] {(IsDESC)}
            LIMIT 1;";

        T res = (await Database.QueryAsync<T>(query)).First();
        return res.Date.Year;
    }
}
