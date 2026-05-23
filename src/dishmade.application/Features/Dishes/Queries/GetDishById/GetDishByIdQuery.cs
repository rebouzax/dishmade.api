using dishmade.application.Features.Dishes.Queries;
using MediatR;

namespace dishmade.application.Features.Dishes.Queries.GetDishById;

public sealed record GetDishByIdQuery(Guid Id) : IRequest<DishResponse>;