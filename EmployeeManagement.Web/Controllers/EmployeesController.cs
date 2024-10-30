using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Application.Features.Employees.Commands;
using EmployeeManagement.Application.Features.Employees.Queries;
using EmployeeManagement.Application.Features.Employees.DTOs;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/employees
    [HttpGet]
    public async Task<ActionResult<List<EmployeeDto>>> GetEmployees()
    {
        var query = new GetEmployeesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // GET: api/employees/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int id)
    {
        var query = new GetEmployeeByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(); // Employé non trouvé
        }

        return Ok(result);
    }

    // POST: api/employees
    [HttpPost]
    public async Task<ActionResult<int>> CreateEmployee(CreateEmployeeCommand command)
    {
        var employeeId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetEmployeeById), new { id = employeeId }, employeeId);
    }

    // PUT: api/employees/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateEmployee(int id, UpdateEmployeeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(); // ID ne correspond pas
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(); // Employé non trouvé
        }

        return NoContent(); // Mise à jour réussie
    }

    // DELETE: api/employees/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEmployee(int id)
    {
        var command = new DeleteEmployeeCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(); // Employé non trouvé
        }

        return NoContent(); // Suppression réussie
    }
}
