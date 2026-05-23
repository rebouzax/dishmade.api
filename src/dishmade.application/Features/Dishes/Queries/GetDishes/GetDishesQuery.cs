using dishmade.application.Features.Dishes.Queries;
using MediatR;

namespace dishmade.application.Features.Dishes.Queries.GetDishes;

public sealed record GetDishesQuery : IRequest<IReadOnlyList<DishResponse>>;