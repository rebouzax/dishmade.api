using dishmade.application.Features.Admin.Clients.Commands.CreateClient;
using dishmade.application.Features.Admin.Clients.Queries.GetClients;
using dishmade.domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dishmade.api.Controllers;

[ApiController]
[Route("api/admin/clients")]
[Authorize(Roles = Roles.PlatformAdmin)]
public sealed class AdminClientsController : ControllerBase
{
    private readonly ISender _sender;

    public AdminClientsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateClientCommand command,
        CancellationToken cancellationToken)
    {
        var userId = await _sender.Send(command, cancellationToken);

        return Created(string.Empty, new { id = userId });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] bool? isActive,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var clients = await _sender.Send(
            new GetClientsQuery(search, isActive, pageNumber, pageSize),
            cancellationToken);

        return Ok(clients);
    }
}