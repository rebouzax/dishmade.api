using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Public.Menu.Queries.GetPublicMenu;
using MediatR;

namespace dishmade.application.Features.Public.Menu.Queries.GetPublicDishes;

public sealed class GetPublicDishesQueryHandler
    : IRequestHandler<GetPublicDishesQuery, IReadOnlyList<PublicDishResponse>>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IDishRepository _dishRepository;

    public GetPublicDishesQueryHandler(
        IRestaurantRepository restaurantRepository,
        IDishRepository dishRepository)
    {
        _restaurantRepository = restaurantRepository;
        _dishRepository = dishRepository;
    }

    public async Task<IReadOnlyList<PublicDishResponse>> Handle(
        GetPublicDishesQuery request,
        CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetBySlugAsync(
            request.Slug,
            cancellationToken);

        if (restaurant is null || !restaurant.IsActive)
            throw new KeyNotFoundException("Restaurante não encontrado.");

        var dishes = await _dishRepository.GetPublicByRestaurantIdAsync(
            restaurant.Id,
            request.CategoryId,
            cancellationToken);

        return dishes
            .Select(dish => new PublicDishResponse(
                dish.Id,
                dish.Name,
                dish.Description,
                dish.Price,
                dish.CategoryId,
                dish.Category.Name,
                $"/api/public/dishes/{dish.Id}/image"))
            .ToList();
    }
}