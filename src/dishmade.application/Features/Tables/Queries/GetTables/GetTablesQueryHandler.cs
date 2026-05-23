using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Tables.Queries;
using MediatR;

namespace dishmade.application.Features.Tables.Queries.GetTables;

public sealed class GetTablesQueryHandler
    : IRequestHandler<GetTablesQuery, IReadOnlyList<TableResponse>>
{
    private readonly IRestaurantTableRepository _tableRepository;

    public GetTablesQueryHandler(IRestaurantTableRepository tableRepository)
    {
        _tableRepository = tableRepository;
    }

    public async Task<IReadOnlyList<TableResponse>> Handle(
        GetTablesQuery request,
        CancellationToken cancellationToken)
    {
        var tables = await _tableRepository.GetAllAsync(cancellationToken);

        return tables
            .Select(table => new TableResponse(
                table.Id,
                table.Number,
                table.IsOccupied,
                table.CreatedAt,
                table.UpdatedAt))
            .ToList();
    }
}