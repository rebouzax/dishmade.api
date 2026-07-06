using dishmade.application.Features.Public.ServiceRequests.Commands.CreatePublicServiceRequest;
using dishmade.domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dishmade.api.Controllers;

[ApiController]
[Route("api/public/service-requests")]
[AllowAnonymous]
public sealed class PublicServiceRequestsController : ControllerBase
{
    private readonly ISender _sender;

    public PublicServiceRequestsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePublicServiceRequestRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new CreatePublicServiceRequestCommand(
                request.RestaurantSlug,
                request.TableNumber,
                request.Type,
                request.Message),
            cancellationToken);

        return Created(string.Empty, response);
    }
}

public sealed record CreatePublicServiceRequestRequest(
    string RestaurantSlug,
    int TableNumber,
    ServiceRequestType Type,
    string? Message
);