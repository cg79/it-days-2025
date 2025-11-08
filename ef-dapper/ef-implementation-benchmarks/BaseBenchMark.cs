using dapper_contrib_implementation;
using dapper_dommel_implementation;
using dapper_implementation;
using dapper_simple_crud_implementation;
using ef_base_repository;
using ef_implementation_tests;
using ef_implementation;
using linq_to_db_implementation;
using repodb_implementation;

namespace ef_implementation_benchmarks;

public class BaseBenchMark: BaseTest
{
    // protected UserServiceEF _userServiceEf;
    // protected UserService_Dapper _userServiceDapper;
    // protected UserService_DapperContrib _userServiceContrib;
    // protected UserService_Dommel _userServiceDommel;
    // protected UserService_RepoDb _userServiceRepoDb;
    // protected UserService_LinqToDb _userServiceLinqToDb;
    // protected UserService_SimpleCrud _userServiceSimpleCrud;
    private IEFDataContext _dbContext;

    public void Setup()
    {
        var dbContext = GetMySqlDbContext();
        this._dbContext = dbContext;
        // _userServiceEf = new UserServiceEF(dbContext);
        // _userServiceDapper = new UserService_Dapper(dbContext);
        // _userServiceContrib = new UserService_DapperContrib(dbContext);
        // _userServiceDommel = new UserService_Dommel(dbContext);
        // _userServiceRepoDb = new UserService_RepoDb(dbContext);
        // _userServiceLinqToDb = new UserService_LinqToDb(dbContext);
        // _userServiceSimpleCrud = new UserService_SimpleCrud(dbContext);
    }
    // private UserServiceEF _userServiceEf;
    public UserServiceEF _userServiceEf
    {
        get { return new UserServiceEF(_dbContext); }
    }

    public UserService_Dapper UserServiceDapper
    {
        get { return new UserService_Dapper(_dbContext); }
    }
    
    public ProductService_Dapper ProductServiceDapper
    {
        get { return new ProductService_Dapper(_dbContext); }
    }

    public UserService_DapperContrib UserServiceContrib
    {
        get { return new UserService_DapperContrib(_dbContext); }
    }

    public UserService_Dommel UserServiceDommel
    {
        get { return new UserService_Dommel(_dbContext); }
    }

    public UserService_RepoDb UserServiceRepoDb
    {
        get { return new UserService_RepoDb(_dbContext); }
    }

    public ProductService_RepoDb ProductService_RepoDb
    {
        get { return new ProductService_RepoDb(_dbContext); }
    }
    
    public ProductService_Dommel ProductService_Dommel
    {
        get { return new ProductService_Dommel(_dbContext); }
    }
    public UserService_LinqToDb UserServiceLinqToDb
    {
        get { return new UserService_LinqToDb(_dbContext); }
    }

    public UserService_SimpleCrud UserServiceSimpleCrud
    {
        get { return new UserService_SimpleCrud(_dbContext); }
    }

}