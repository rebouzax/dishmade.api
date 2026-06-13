using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Tables.Queries;
using MediatR;

namespace dishmade.application.Features.Tables.Queries.GetTableById;

public sealed class GetTableByIdQueryHandler : IRequestHandler<GetTableByIdQuery, TableResponse>
{
    private readonly IRestaurantTableRepository _tableRepository;

    public GetTableByIdQueryHandler(IRestaurantTableRepository tableRepository)
    {
        _tableRepository = tableRepository;
    }

    public async Task<TableResponse> Handle(
        GetTableByIdQuery request,
        CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByIdAsync(request.Id, cancellationToken);

        if (table is null)
            throw new KeyNotFoundException("Mesa não encontrada.");

        return new TableResponse(
            table.Id,
            table.Number,
            table.IsOccupied,
            table.IsMenuQrCodeEnabled,
            table.MenuQrCodeEnabledAt,
            table.CreatedAt,
            table.UpdatedAt);
    }
}