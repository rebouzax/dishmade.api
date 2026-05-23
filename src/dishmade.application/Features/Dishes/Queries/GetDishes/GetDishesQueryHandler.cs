using dishmade.application.Abstractions.Repositories;
using dishmade.application.Common.Pagination;
using MediatR;

namespace dishmade.application.Features.Dishes.Queries.GetDishes;

public sealed class GetDishesQueryHandler
    : IRequestHandler<GetDishesQuery, PagedResponse<DishResponse>>
{
    private readonly IDishRepository _dishRepository;

    public GetDishesQueryHandler(IDishRepository dishRepository)
    {
        _dishRepository = dishRepository;
    }

    public async Task<PagedResponse<DishResponse>> Handle(
        GetDishesQuery request,
        CancellationToken cancellationToken)
    {
        var pageNumber = PaginationHelper.NormalizePageNumber(request.PageNumber);
        var pageSize = PaginationHelper.NormalizePageSize(request.PageSize);

        var result = await _dishRepository.GetPagedAsync(
            request.Search,
            request.CategoryId,
            request.IsAvailable,
            pageNumber,
            pageSize,
            cancellationToken);

        var dishes = result.Items
            .Select(dish => new DishResponse(
                dish.Id,
                dish.Name,
                dish.Description,
                dish.Price,
                dish.IsAvailable,
                dish.CategoryId,
                dish.Category.Name,
                dish.CreatedAt,
                dish.UpdatedAt))
            .ToList();

        return new PagedResponse<DishResponse>(
            dishes,
            pageNumber,
            pageSize,
            result.TotalCount);
    }
}