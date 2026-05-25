using dishmade.application.Abstractions.Auth;
using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHashService passwordHashService,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _passwordHashService = passwordHashService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Usuário inativo.");

        if (user.Restaurant is not null && !user.Restaurant.IsActive)
            throw new UnauthorizedAccessException("Restaurante inativo.");

        var passwordIsValid = _passwordHashService.VerifyPassword(user, request.Password);

        if (!passwordIsValid)
            throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

        var token = _jwtTokenService.GenerateToken(user);
        var expiresAt = _jwtTokenService.GetExpirationDate();

        return new AuthResponse(
            token,
            expiresAt,
            new UserAuthResponse(
                user.Id,
                user.Name,
                user.Email,
                user.Role,
                user.RestaurantId,
                user.Restaurant?.Name));
    }
}