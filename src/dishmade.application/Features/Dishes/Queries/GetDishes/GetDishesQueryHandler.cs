using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Dishes.Queries.GetDishes;

public sealed class GetDishesQueryHandler
    : IRequestHandler<GetDishesQuery, IReadOnlyList<DishResponse>>
{
    private readonly IDishRepository _dishRepository;

    public GetDishesQueryHandler(IDishRepository dishRepository)
    {
        _dishRepository = dishRepository;
    }

    public async Task<IReadOnlyList<DishResponse>> Handle(
        GetDishesQuery request,
        CancellationToken cancellationToken)
    {
        var dishes = await _dishRepository.GetAllAsync(cancellationToken);

        return dishes
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
    }
}