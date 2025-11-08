using ef_base_repository;
using ef_dapper_models;
using System.Data;
using Dapper;
using Dommel;
using Efcom.Base.Repository.Request;

namespace dapper_dommel_implementation;

public partial class UserService_Dommel
{
    public async Task<PaginationResponse<User>> Pagination(PaginationRequest paginationRequest)
    {
        await using var db = await GetOpenConnectionAsync();
         var result = await db.PaginateAsync<User>(paginationRequest);
         return result;
    }
}
