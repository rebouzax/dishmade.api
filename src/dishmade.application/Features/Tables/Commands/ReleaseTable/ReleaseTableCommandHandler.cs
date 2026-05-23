using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Tables.Commands.ReleaseTable;

public sealed class ReleaseTableCommandHandler : IRequestHandler<ReleaseTableCommand>
{
    private readonly IRestaurantTableRepository _tableRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReleaseTableCommandHandler(
        IRestaurantTableRepository tableRepository,
        IUnitOfWork unitOfWork)
    {
        _tableRepository = tableRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ReleaseTableCommand request,
        CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByIdAsync(request.Id, cancellationToken);

        if (table is null)
            throw new KeyNotFoundException("Mesa não encontrada.");

        table.Release();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}