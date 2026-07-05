using dishmade.application.Abstractions.Repositories;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace dishmade.application.Features.Public.Menu.Queries.GetPublicMenu;

public sealed class GetPublicMenuQueryHandler : IRequestHandler<GetPublicMenuQuery, PublicMenuResponse>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDishRepository _dishRepository;
    private readonly IDishOptionGroupRepository _dishOptionGroupRepository;
    private readonly IConfiguration _configuration;

    public GetPublicMenuQueryHandler(
        IRestaurantRepository restaurantRepository,
        ICategoryRepository categoryRepository,
        IDishRepository dishRepository,
        IDishOptionGroupRepository dishOptionGroupRepository,
        IConfiguration configuration)
    {
        _restaurantRepository = restaurantRepository;
        _categoryRepository = categoryRepository;
        _dishRepository = dishRepository;
        _dishOptionGroupRepository = dishOptionGroupRepository;
        _configuration = configuration;
    }

    public async Task<PublicMenuResponse> Handle(
        GetPublicMenuQuery request,
        CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetBySlugAsync(
            request.Slug,
            cancellationToken);

        if (restaurant is null || !restaurant.IsActive)
            throw new KeyNotFoundException("Restaurante não encontrado.");

        var categories = await _categoryRepository.GetPublicByRestaurantIdAsync(
            restaurant.Id,
            cancellationToken);

        var dishes = await _dishRepository.GetPublicByRestaurantIdAsync(
            restaurant.Id,
            categoryId: null,
            cancellationToken);

        var baseUrl = _configuration["PublicMenu:BaseUrl"]?.TrimEnd('/')
            ?? "http://localhost:3000/menu";

        var menuUrl = $"{baseUrl}/{restaurant.Slug}";
        var qrCodeUrl = $"/api/public/restaurants/{restaurant.Slug}/qr-code";

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

        var responseCategories = categories
            .Select(category => new PublicCategoryResponse(
                category.Id,
                category.Name,
                category.Description,
                dishResponses
                    .Where(d => d.CategoryId == category.Id)
                    .ToList()))
            .Where(category => category.Dishes.Count > 0)
            .ToList();

        return new PublicMenuResponse(
            restaurant.Id,
            restaurant.Name,
            restaurant.Slug,
            menuUrl,
            qrCodeUrl,
            responseCategories);
    }
}