using System.Data;
using System.Linq.Expressions;
using ef_base_repository;
using ef_dapper_models;
using Efcom.Base.Repository.Request;
using LinqToDB;

namespace ef_implementation;

public partial class UserServiceEF : GenericRepository<User>
{
    private IEFDataContext _dataContext;
    public UserServiceEF(IEFDataContext dataContext) : base(dataContext)
    {
        _dataContext = dataContext;
    }


    public async Task<User> Insert(User user)
    {
        await using var connection = _dataContext.GetDbConnection();

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        await using var transaction = await _dataContext.Database.BeginTransactionAsync();

        try
        {
            var userRepo = new BaseRepository<User>(_dataContext);
            var response = await userRepo.InsertAsync(user);

            var invoiceRepo = new BaseRepository<Invoice>(_dataContext);
            var invResponse = await invoiceRepo.InsertAsync(new Invoice()
            {
                UserId = response.Id,
                TotalAmount = 3,
            });

            // confirmă tranzacția
            await transaction.CommitAsync();

            return response;
        }
        catch (Exception ex)
        {
            // rollback dacă apare eroare
            await transaction.RollbackAsync();
            Console.WriteLine($"Insert failed: {ex.Message}");
            throw;
        }
    }

    
    public async Task<User> InsertNoTracking(User user)
    {
        BaseRepository<User> userRepo = new BaseRepository<User>(_dataContext);
        User response = await userRepo.InsertAsyncNoTracking(user);
        return response;
    }

    public async Task<IEnumerable<User>> Find(QueryFilter filter)
    {
            GenericRepository<User> userRepo = new GenericRepository<User>(_dataContext);
            var response =  await userRepo.FindQueryFilter(filter).ToListAsync();
            return response;
    }

    public IQueryable<User> FindWithLinq(Expression<Func<User, bool>> whereExpression)
    {
        return _dataContext.Set<User>().Where(whereExpression);
    }

    public async Task<List<User>> Filter(FilterGroup group)
    {
        try
        {
            GenericRepository<User> userRepo = new GenericRepository<User>(_dataContext);
            var response = await  userRepo.Filter(group).ToListAsync();
            return response;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<List<Invoice>> getInvoices(QueryFilter group)
    {
        InvoiceRepository invoiceRepo = new InvoiceRepository(_dataContext);
        var response = await invoiceRepo.GetInvoicesAsync(group);
        return response;
    }

    public async Task<List<Invoice>> getInvoicesWithMultipleQueries(QueryFilter group)
    {
        InvoiceRepository invoiceRepo = new InvoiceRepository(_dataContext);
        var response = await invoiceRepo.GetInvoicesAsyncMultipleQueries(group);
        return response;
    }

    public async Task<PaginationResponse<User>> PaginatedUsers(PaginationRequest request)
    {
        GenericRepository<User> userRepo = new GenericRepository<User>(_dataContext);
        var response = await userRepo.GetPagedAsync(request);
        return response;
    }

}
