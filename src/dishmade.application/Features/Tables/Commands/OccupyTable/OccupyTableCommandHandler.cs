using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Tables.Commands.OccupyTable;

public sealed class OccupyTableCommandHandler : IRequestHandler<OccupyTableCommand>
{
    private readonly IRestaurantTableRepository _tableRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OccupyTableCommandHandler(
        IRestaurantTableRepository tableRepository,
        IUnitOfWork unitOfWork)
    {
        _tableRepository = tableRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        OccupyTableCommand request,
        CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByIdAsync(request.Id, cancellationToken);

        if (table is null)
            throw new KeyNotFoundException("Mesa não encontrada.");

        table.Occupy();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}