using dishmade.application.Features.SalesHistory.Queries.GetSalesHistory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace dishmade.api.Controllers;

[ApiController]
[Route("api/sales-history")]
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
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetSalesHistoryQuery(startDate, endDate),
            cancellationToken);

        return Ok(result);
    }
}