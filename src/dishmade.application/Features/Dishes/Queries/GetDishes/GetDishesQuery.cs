using dishmade.application.Common.Pagination;
using dishmade.application.Features.Dishes.Queries;
using MediatR;

namespace dishmade.application.Features.Dishes.Queries.GetDishes;

public sealed record GetDishesQuery(
    string? Search,
    Guid? CategoryId,
    bool? IsAvailable,
    int PageNumber,
    int PageSize
) : IRequest<PagedResponse<DishResponse>>;