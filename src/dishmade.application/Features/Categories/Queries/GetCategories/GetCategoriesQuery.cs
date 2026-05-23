using dishmade.application.Common.Pagination;
using MediatR;

namespace dishmade.application.Features.Categories.Queries.GetCategories;

public sealed record GetCategoriesQuery(
    string? Search,
    bool? IsActive,
    int PageNumber,
    int PageSize
) : IRequest<PagedResponse<CategoryResponse>>;