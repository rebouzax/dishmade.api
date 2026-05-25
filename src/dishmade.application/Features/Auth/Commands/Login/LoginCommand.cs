using MediatR;

namespace dishmade.application.Features.Auth.Commands.Login;

public sealed record LoginCommand(
    string Email,
    string Password
) : IRequest<AuthResponse>;