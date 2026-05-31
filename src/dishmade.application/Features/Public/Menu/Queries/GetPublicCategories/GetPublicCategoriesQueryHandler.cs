using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Public.Menu.Queries.GetPublicMenu;
using MediatR;

namespace dishmade.application.Features.Public.Menu.Queries.GetPublicCategories;

public sealed class GetPublicCategoriesQueryHandler
    : IRequestHandler<GetPublicCategoriesQuery, IReadOnlyList<PublicCategoryResponse>>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly ICategoryRepository _categoryRepository;

    public GetPublicCategoriesQueryHandler(
        IRestaurantRepository restaurantRepository,
        ICategoryRepository categoryRepository)
    {
        _restaurantRepository = restaurantRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyList<PublicCategoryResponse>> Handle(
        GetPublicCategoriesQuery request,
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

        return categories
            .Select(category => new PublicCategoryResponse(
                category.Id,
                category.Name,
                category.Description,
                []))
            .ToList();
    }
}