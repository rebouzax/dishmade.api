using dishmade.application.Abstractions.Auth;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Tables.MenuQrCode;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace dishmade.application.Features.Tables.Queries.GetTableMenuQrCode;

public sealed class GetTableMenuQrCodeQueryHandler
    : IRequestHandler<GetTableMenuQrCodeQuery, TableMenuQrCodeResponse>
{
    private readonly IRestaurantTableRepository _tableRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IConfiguration _configuration;

    public GetTableMenuQrCodeQueryHandler(
        IRestaurantTableRepository tableRepository,
        IRestaurantRepository restaurantRepository,
        ICurrentUserService currentUserService,
        IConfiguration configuration)
    {
        _tableRepository = tableRepository;
        _restaurantRepository = restaurantRepository;
        _currentUserService = currentUserService;
        _configuration = configuration;
    }

    public async Task<TableMenuQrCodeResponse> Handle(
        GetTableMenuQrCodeQuery request,
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

        return TableMenuQrCodeResponseFactory.Create(
            table,
            restaurant,
            _configuration);
    }
}