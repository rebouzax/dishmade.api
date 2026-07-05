using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Public.Menu.Queries.GetPublicMenu;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace dishmade.application.Features.Public.Menu.Queries.GetPublicDishes;

public sealed class GetPublicDishesQueryHandler
    : IRequestHandler<GetPublicDishesQuery, IReadOnlyList<PublicDishResponse>>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IDishRepository _dishRepository;
    private readonly IDishOptionGroupRepository _dishOptionGroupRepository;

    public GetPublicDishesQueryHandler(
        IRestaurantRepository restaurantRepository,
        IDishRepository dishRepository,
        IDishOptionGroupRepository dishOptionGroupRepository)
    {
        _restaurantRepository = restaurantRepository;
        _dishRepository = dishRepository;
        _dishOptionGroupRepository = dishOptionGroupRepository;
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

        var dishResponses = new List<PublicDishResponse>();

        foreach (var dish in dishes)
        {
            var optionGroups = await _dishOptionGroupRepository.GetPublicByDishIdAsync(
                dish.Id,
                restaurant.Id,
                cancellationToken);

            var optionGroupResponses = optionGroups
                .Select(group => new PublicDishOptionGroupResponse(
                    group.Id,
                    group.Name,
                    group.IsRequired,
                    group.MinSelection,
                    group.MaxSelection,
                    group.Options
                        .Where(option => option.IsAvailable && !option.IsDeleted)
                        .Select(option => new PublicDishOptionResponse(
                            option.Id,
                            option.Name,
                            option.AdditionalPrice))
                        .ToList()))
                .ToList();

            dishResponses.Add(new PublicDishResponse(
                dish.Id,
                dish.Name,
                dish.Description,
                dish.Price,
                dish.CategoryId,
                dish.Category.Name,
                $"/api/public/dishes/{dish.Id}/image",
                optionGroupResponses));
        }

        return dishResponses;
    }
}