using dishmade.application.Abstractions.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace dishmade.application.Features.Public.Menu.Queries.GetPublicMenu;

public sealed class GetPublicMenuQueryHandler : IRequestHandler<GetPublicMenuQuery, PublicMenuResponse>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDishRepository _dishRepository;
    private readonly IConfiguration _configuration;

    public GetPublicMenuQueryHandler(
        IRestaurantRepository restaurantRepository,
        ICategoryRepository categoryRepository,
        IDishRepository dishRepository,
        IConfiguration configuration)
    {
        _restaurantRepository = restaurantRepository;
        _categoryRepository = categoryRepository;
        _dishRepository = dishRepository;
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

        var responseCategories = categories
            .Select(category => new PublicCategoryResponse(
                category.Id,
                category.Name,
                category.Description,
                dishes
                    .Where(dish => dish.CategoryId == category.Id)
                    .Select(dish => new PublicDishResponse(
                        dish.Id,
                        dish.Name,
                        dish.Description,
                        dish.Price,
                        dish.CategoryId,
                        category.Name,
                        $"/api/public/dishes/{dish.Id}/image"))
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