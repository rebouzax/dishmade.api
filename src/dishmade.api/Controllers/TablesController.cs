using dishmade.application.Features.Tables.Commands.CreateTable;
using dishmade.application.Features.Tables.Commands.DeleteTable;
using dishmade.application.Features.Tables.Commands.OccupyTable;
using dishmade.application.Features.Tables.Commands.ReleaseTable;
using dishmade.application.Features.Tables.Commands.UpdateTable;
using dishmade.application.Features.Tables.Queries.GetTableById;
using dishmade.application.Features.Tables.Queries.GetTables;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using dishmade.domain.Constants;
using Microsoft.AspNetCore.Authorization;

namespace dishmade.api.Controllers;

[ApiController]
[Route("api/tables")]
[Authorize(Roles = Roles.Client)]
public sealed class TablesController : ControllerBase
{
    private readonly ISender _sender;

    public TablesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTableRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateTableCommand(request.Number);

        var tableId = await _sender.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = tableId },
            new { id = tableId });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] int? number,
    [FromQuery] bool? isOccupied,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken cancellationToken = default)
    {
        var tables = await _sender.Send(
            new GetTablesQuery(number, isOccupied, pageNumber, pageSize),
            cancellationToken);

        return Ok(tables);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var table = await _sender.Send(new GetTableByIdQuery(id), cancellationToken);

        return Ok(table);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateTableRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTableCommand(id, request.Number);

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteTableCommand(id), cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:guid}/occupy")]
    public async Task<IActionResult> Occupy(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(new OccupyTableCommand(id), cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:guid}/release")]
    public async Task<IActionResult> Release(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(new ReleaseTableCommand(id), cancellationToken);

        return NoContent();
    }
}

public sealed record CreateTableRequest(int Number);

public sealed record UpdateTableRequest(int Number);