using dishmade.application.Features.Categories.Commands.CreateCategory;
using dishmade.application.Features.Categories.Queries.GetCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using dishmade.domain.Constants;
using Microsoft.AspNetCore.Authorization;

namespace dishmade.api.Controllers;

[ApiController]
[Route("api/categories")]
[Authorize(Roles = Roles.Client)]
public sealed class CategoriesController : ControllerBase
{
    private readonly ISender _sender;

    public CategoriesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var categoryId = await _sender.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetAll),
            new { id = categoryId },
            new { id = categoryId });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] string? search,
    [FromQuery] bool? isActive,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken cancellationToken = default)
    {
        var categories = await _sender.Send(
            new GetCategoriesQuery(search, isActive, pageNumber, pageSize),
            cancellationToken);

        return Ok(categories);
    }
}