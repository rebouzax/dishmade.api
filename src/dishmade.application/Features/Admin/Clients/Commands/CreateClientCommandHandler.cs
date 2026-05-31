using dishmade.application.Abstractions.Auth;
using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Common.Slugs;
using dishmade.domain.Entities;
using MediatR;

namespace dishmade.application.Features.Admin.Clients.Commands.CreateClient;

public sealed class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Guid>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateClientCommandHandler(
        IRestaurantRepository restaurantRepository,
        IUserRepository userRepository,
        IPasswordHashService passwordHashService,
        IUnitOfWork unitOfWork)
    {
        _restaurantRepository = restaurantRepository;
        _userRepository = userRepository;
        _passwordHashService = passwordHashService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateClientCommand request,
        CancellationToken cancellationToken)
    {
        var emailAlreadyExists = await _userRepository.ExistsByEmailAsync(
            request.Email,
            cancellationToken);

        if (emailAlreadyExists)
            throw new InvalidOperationException("Já existe um usuário com esse e-mail.");

        var baseSlug = SlugHelper.Generate(request.RestaurantName);
        var slug = baseSlug;

        var slugAlreadyExists = await _restaurantRepository.ExistsBySlugAsync(
            slug,
            cancellationToken);

        if (slugAlreadyExists)
            slug = $"{baseSlug}-{Guid.NewGuid().ToString()[..8]}";

        var restaurant = new Restaurant(
            request.RestaurantName,
            request.RestaurantDocument,
            slug);

        var user = AppUser.CreateClient(
            request.UserName,
            request.Email,
            restaurant.Id);

        var passwordHash = _passwordHashService.HashPassword(user, request.Password);

        user.SetPasswordHash(passwordHash);

        await _restaurantRepository.AddAsync(restaurant, cancellationToken);
        await _userRepository.AddAsync(user, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}