using System.Diagnostics;
using ef_dapper_models;
using ef_dapper.commands;
using ef_implementation;
using Microsoft.AspNetCore.Mvc;

namespace ef_dapper;

[ApiController]
[Route("api/[controller]")]
public class UserController: ControllerBase
{

    // private readonly IUserServiceFactory _factory;

    // public UserController(IUserServiceFactory factory)
    // {
    //     _factory = factory;
    // }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateUserEF([FromBody] User command)
    {
        return Ok(1);
        // Debugger.Break();
        // try
        // {
        //     var _userService = _factory.GetService(UserServiceType.EF);
        //     var resp = await _userService.Insert(command);
        //     return Ok(resp);
        // }
        // catch (Exception ex)
        // {
        //     return BadRequest(ex.Message);
        // }
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateUserDapper([FromBody] User command)
    {
        return Ok(1);
        // var _userService = _factory.GetService(UserServiceType.Dapper);
        // var resp = await _userService.Insert(command);
        // return Ok(resp);
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateUserDapper_simplecrud([FromBody] User command)
    {
        // var _userService = _factory.GetService(UserServiceType.SimpleCrud);
        // var resp = await _userService.Insert(command);
        return Ok(1);
    }
    
    // [HttpPost("[action]")]
    // public async Task<IActionResult> CreateUserDapper_LinqToDb([FromBody] User command)
    // {
    //     var _userService = _factory.GetService(UserServiceType.LinqToDb);
    //     var resp = await _userService.Insert(command);
    //     return Ok(resp);
    // }
    

    [HttpGet("pagination")]
    public async Task<IActionResult> GetPagedUsers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? filterExpression = null
    )
    {
        
        return Ok();
    }
}