using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Dishes.Queries.GetDishById;

public sealed class GetDishByIdQueryHandler : IRequestHandler<GetDishByIdQuery, DishResponse>
{
    private readonly IDishRepository _dishRepository;

    public GetDishByIdQueryHandler(IDishRepository dishRepository)
    {
        _dishRepository = dishRepository;
    }

    public async Task<DishResponse> Handle(
        GetDishByIdQuery request,
        CancellationToken cancellationToken)
    {
        var dish = await _dishRepository.GetByIdAsync(request.Id, cancellationToken);

        if (dish is null)
            throw new KeyNotFoundException("Prato não encontrado.");

        return new DishResponse(
            dish.Id,
            dish.Name,
            dish.Description,
            dish.Price,
            dish.IsAvailable,
            dish.CategoryId,
            dish.Category.Name,
            dish.CreatedAt,
            dish.UpdatedAt);
    }
}