using SQLite;
using Helper.Models;

namespace Helper.Data;

public class HelperDatabase
{
    private SQLiteAsyncConnection Database;

    public HelperDatabase()
    {
        //DropTables();
        CreateTables();
    }

    private void CreateTables()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        Database.CreateTableAsync<Operation>();
    }

    private void DropTables()
    {
        if (Database is not null)
            return;

        try
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            Database.DropTableAsync<Operation>();
        }
        catch { }
    }

    public async Task<List<Operation>> GetOperationsAsync()
    {
        return await Database.Table<Operation>().ToListAsync();
    }

    public async Task<Operation> GetOperationAsync(int id)
    {
        return await Database.Table<Operation>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveOperationAsync(Operation operation)
    {
        if (operation.Id != 0)
            return await Database.UpdateAsync(operation);
        else
            return await Database.InsertAsync(operation);
    }

    public async Task<int> DeleteOperationAsync(Operation operation)
    {
        return await Database.DeleteAsync(operation);
    }
}
