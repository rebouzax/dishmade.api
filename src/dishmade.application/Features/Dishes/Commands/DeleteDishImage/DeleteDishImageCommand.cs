using MediatR;

namespace dishmade.application.Features.Dishes.Commands.DeleteDishImage;

public sealed record DeleteDishImageCommand(Guid DishId) : IRequest;