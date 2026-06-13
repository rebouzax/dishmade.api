using dishmade.application.Abstractions.Auth;
using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Tables.MenuQrCode;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace dishmade.application.Features.Tables.Commands.EnableTableMenuQrCode;

public sealed class EnableTableMenuQrCodeCommandHandler
    : IRequestHandler<EnableTableMenuQrCodeCommand, TableMenuQrCodeResponse>
{
    private readonly IRestaurantTableRepository _tableRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public EnableTableMenuQrCodeCommandHandler(
        IRestaurantTableRepository tableRepository,
        IRestaurantRepository restaurantRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _tableRepository = tableRepository;
        _restaurantRepository = restaurantRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<TableMenuQrCodeResponse> Handle(
        EnableTableMenuQrCodeCommand request,
        CancellationToken cancellationToken)
    {
        var restaurantId = _currentUserService.GetRequiredRestaurantId();

        var table = await _tableRepository.GetByIdAsync(
            request.TableId,
            cancellationToken);

        if (table is null)
            throw new KeyNotFoundException("Mesa não encontrada.");

        var restaurant = await _restaurantRepository.GetByIdAsync(
            restaurantId,
            cancellationToken);

        if (restaurant is null || !restaurant.IsActive)
            throw new KeyNotFoundException("Restaurante não encontrado.");

        table.EnableMenuQrCode();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return TableMenuQrCodeResponseFactory.Create(
            table,
            restaurant,
            _configuration);
    }
}