using dishmade.application.Features.SalesHistory.Queries.GetSalesHistory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using dishmade.domain.Constants;
using Microsoft.AspNetCore.Authorization;

namespace dishmade.api.Controllers;

[ApiController]
[Route("api/sales-history")]
[Authorize(Roles = Roles.Client)]
public sealed class SalesHistoryController : ControllerBase
{
    private readonly ISender _sender;

    public SalesHistoryController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetSalesHistory(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(
            new GetSalesHistoryQuery(startDate, endDate, pageNumber, pageSize),
            cancellationToken);

        return Ok(result);
    }
}