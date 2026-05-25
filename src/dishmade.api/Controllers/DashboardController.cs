using dishmade.application.Features.Dashboard.Queries.GetRestaurantDashboard;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using dishmade.domain.Constants;
using Microsoft.AspNetCore.Authorization;

namespace dishmade.api.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize(Roles = Roles.Client)]
public sealed class DashboardController : ControllerBase
{
    private readonly ISender _sender;

    public DashboardController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboard(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetRestaurantDashboardQuery(startDate, endDate),
            cancellationToken);

        return Ok(result);
    }
}