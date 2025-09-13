using ef_base_repository;
using ef_dapper_models;
using LinqToDB;

namespace ef_implementation;

public class UserServiceEF 
{
    private IEFDataContext _dataContext;
    public UserServiceEF(IEFDataContext dataContext)
    {
        this._dataContext = dataContext;
    }
    public async Task<User> Insert(User user, bool tracking = true)
    {
        try
        {
            BaseRepository<User> userRepo = new BaseRepository<User>(_dataContext);
            User response = await userRepo.InsertAsync(user, tracking );
            return response;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    public async Task<IEnumerable<User>> Find(QueryFilter filter)
    {
        try
        {
            GenericRepository<User> userRepo = new GenericRepository<User>(_dataContext);
            var response =  await userRepo.Find(filter).ToListAsync();
            return response;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    public async Task<List<User>> Filter(FilterGroup group)
    {
        try
        {
            GenericRepository<User> userRepo = new GenericRepository<User>(_dataContext);
            var response = await userRepo.Filter(group);
            return response;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    public async Task<List<Invoice>> getInvoices(QueryFilter group)
    {
        try
        {
            InvoiceRepository invoiceRepo = new InvoiceRepository(_dataContext);
            var response = await invoiceRepo.GetInvoicesAsync(group);
            return response;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    public async Task<List<Invoice>> getInvoicesWithMultipleQueries(QueryFilter group)
    {
        try
        {
            InvoiceRepository invoiceRepo = new InvoiceRepository(_dataContext);
            var response = await invoiceRepo.GetInvoicesAsyncMultipleQueries(group);
            return response;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    
   
}
