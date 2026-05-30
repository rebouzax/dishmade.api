using MediatR;

namespace dishmade.application.Features.Dishes.Queries.GetDishImage;

public sealed record GetDishImageQuery(Guid DishId) : IRequest<DishImageResponse>;