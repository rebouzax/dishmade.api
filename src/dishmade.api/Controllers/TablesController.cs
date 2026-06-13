using dishmade.application.Features.Tables.Commands.CreateTable;
using dishmade.application.Features.Tables.Commands.DeleteTable;
using dishmade.application.Features.Tables.Commands.DisableTableMenuQrCode;
using dishmade.application.Features.Tables.Commands.EnableTableMenuQrCode;
using dishmade.application.Features.Tables.Commands.OccupyTable;
using dishmade.application.Features.Tables.Commands.ReleaseTable;
using dishmade.application.Features.Tables.Commands.UpdateTable;
using dishmade.application.Features.Tables.Queries.GetTableById;
using dishmade.application.Features.Tables.Queries.GetTableMenuQrCode;
using dishmade.application.Features.Tables.Queries.GetTables;
using dishmade.domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

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

    [HttpPatch("{id:guid}/menu-qr-code/enable")]
    public async Task<IActionResult> EnableMenuQrCode(
    Guid id,
    CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new EnableTableMenuQrCodeCommand(id),
            cancellationToken);

        return Ok(response);
    }

    [HttpPatch("{id:guid}/menu-qr-code/disable")]
    public async Task<IActionResult> DisableMenuQrCode(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DisableTableMenuQrCodeCommand(id),
            cancellationToken);

        return NoContent();
    }

    [HttpGet("{id:guid}/menu-qr-code")]
    public async Task<IActionResult> GetMenuQrCode(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new GetTableMenuQrCodeQuery(id),
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:guid}/menu-qr-code/image")]
    public async Task<IActionResult> GetMenuQrCodeImage(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new GetTableMenuQrCodeQuery(id),
            cancellationToken);

        if (!response.IsEnabled || string.IsNullOrWhiteSpace(response.MenuUrl))
            return BadRequest(new { message = "O QR Code do cardápio não está habilitado para esta mesa." });

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(
            response.MenuUrl,
            QRCodeGenerator.ECCLevel.Q);

        var pngQrCode = new PngByteQRCode(qrCodeData);
        var qrCodeBytes = pngQrCode.GetGraphic(20);

        return File(
            qrCodeBytes,
            "image/png",
            $"mesa-{response.TableNumber}-{response.RestaurantSlug}-qrcode.png");
    }

}

public sealed record CreateTableRequest(int Number);

public sealed record UpdateTableRequest(int Number);