using StockManagement.Application.Features.Roles.DTOs;
using StockManagement.Application.Features.Roles.Queries;
using StockManagement.Application.Features.Users.Commands; 
using StockManagement.Application.Features.Users.DTOs;
using StockManagement.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UsersRolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersRolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/usersroles
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        var query = new GetUsersQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // GET: api/usersroles/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id)
    {
        var query = new GetUserByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(); // Utilisateur non trouvé
        }

        return Ok(result);
    }

    // POST: api/usersroles
    [HttpPost]
    public async Task<ActionResult<int>> CreateUser(CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetUserById), new { id = userId }, userId);
    }

    // PUT: api/usersroles/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(Guid id, UpdateUserCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(); // ID ne correspond pas
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(); // Utilisateur non trouvé
        }

        return NoContent(); // Mise à jour réussie
    }

    // DELETE: api/usersroles/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        var command = new DeleteUserCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(); // Utilisateur non trouvé
        }

        return NoContent(); // Suppression réussie
    }

    // GET: api/usersroles/roles
    [HttpGet("roles")]
    public async Task<ActionResult<List<RoleDto>>> GetRoles()
    {
        var query = new GetRolesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
