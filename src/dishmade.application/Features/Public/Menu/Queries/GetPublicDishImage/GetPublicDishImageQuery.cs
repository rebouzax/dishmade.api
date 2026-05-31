using dishmade.application.Features.Dishes.Queries.GetDishImage;
using MediatR;

namespace dishmade.application.Features.Public.Menu.Queries.GetPublicDishImage;

public sealed record GetPublicDishImageQuery(Guid DishId) : IRequest<DishImageResponse>;