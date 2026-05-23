using dishmade.application.Features.Categories.Commands.CreateCategory;
using dishmade.application.Features.Categories.Queries.GetCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace dishmade.api.Controllers;

[ApiController]
[Route("api/categories")]
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
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var categories = await _sender.Send(new GetCategoriesQuery(), cancellationToken);

        return Ok(categories);
    }
}