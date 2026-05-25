using MediatR;

namespace dishmade.application.Features.Admin.Clients.Commands.CreateClient;

public sealed record CreateClientCommand(
    string RestaurantName,
    string? RestaurantDocument,
    string UserName,
    string Email,
    string Password
) : IRequest<Guid>;