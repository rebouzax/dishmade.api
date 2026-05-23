using MediatR;

namespace dishmade.application.Features.Dishes.Commands.DeleteDish;

public sealed record DeleteDishCommand(Guid Id) : IRequest;