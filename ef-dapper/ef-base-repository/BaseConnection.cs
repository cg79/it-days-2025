using System.Data;
using System.Data.Common;

namespace ef_base_repository;

public class BaseConnection(IEFDataContext dataContext)
{
    protected async Task<DbConnection> GetOpenConnectionAsync()
    {
        var db = dataContext.GetDbConnection();
        if (db.State != ConnectionState.Open)
            await db.OpenAsync();
        return db;
    }
}