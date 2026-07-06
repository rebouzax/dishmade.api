using dishmade.application.Features.ServiceRequests.Commands.CancelServiceRequest;
using dishmade.application.Features.ServiceRequests.Commands.ResolveServiceRequest;
using dishmade.application.Features.ServiceRequests.Commands.StartServiceRequest;
using dishmade.application.Features.ServiceRequests.Queries.GetServiceRequestById;
using dishmade.application.Features.ServiceRequests.Queries.GetServiceRequests;
using dishmade.domain.Constants;
using dishmade.domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dishmade.api.Controllers;

[ApiController]
[Route("api/service-requests")]
[Authorize(Roles = Roles.Client)]
public sealed class ServiceRequestsController : ControllerBase
{
    private readonly ISender _sender;

    public ServiceRequestsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] ServiceRequestStatus? status,
        [FromQuery] ServiceRequestType? type,
        [FromQuery] Guid? tableId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var response = await _sender.Send(
            new GetServiceRequestsQuery(
                status,
                type,
                tableId,
                pageNumber,
                pageSize),
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new GetServiceRequestByIdQuery(id),
            cancellationToken);

        return Ok(response);
    }

    [HttpPatch("{id:guid}/start")]
    public async Task<IActionResult> Start(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new StartServiceRequestCommand(id),
            cancellationToken);

        return Ok(response);
    }

    [HttpPatch("{id:guid}/resolve")]
    public async Task<IActionResult> Resolve(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new ResolveServiceRequestCommand(id),
            cancellationToken);

        return Ok(response);
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new CancelServiceRequestCommand(id),
            cancellationToken);

        return Ok(response);
    }
}