using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Tables.Commands.DeleteTable;

public sealed class DeleteTableCommandHandler : IRequestHandler<DeleteTableCommand>
{
    private readonly IRestaurantTableRepository _tableRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTableCommandHandler(
        IRestaurantTableRepository tableRepository,
        IUnitOfWork unitOfWork)
    {
        _tableRepository = tableRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeleteTableCommand request,
        CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByIdAsync(request.Id, cancellationToken);

        if (table is null)
            throw new KeyNotFoundException("Mesa não encontrada.");

        table.Delete();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}