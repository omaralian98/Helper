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

        //// Create an index on the Date column for all entities that have a Date property
        //await CreateIndexOnDateAsync();
    }

    //private async Task CreateIndexOnDateAsync()
    //{
    //    var tableName = typeof(T).Name;

    //    var createIndexSql = $"CREATE INDEX IF NOT EXISTS IX_{tableName}_Date ON {tableName} (Date);";

    //    var res = await Database.ExecuteAsync(createIndexSql);
    //}

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

    public async Task<List<T>> GetAllAsync()
    {
        return await Database.Table<T>().ToListAsync();
    }

    public async Task<T> GetAsync(int id)
    {
        return await Database.Table<T>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveAsync(T operation)
    {
        if (operation.Id != 0)
            return await Database.UpdateAsync(operation);
        else
            return await Database.InsertAsync(operation);
    }

    public async Task<int> DeleteAsync(T operation)
    {
        return await Database.DeleteAsync(operation);
    }

    public async Task<int> GetFirst() => await GetFirstOrLast(last: false);

    public async Task<int> GetLast() => await GetFirstOrLast(last: true);

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
