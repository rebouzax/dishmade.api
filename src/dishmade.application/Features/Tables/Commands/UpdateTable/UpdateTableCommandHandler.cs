using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Tables.Commands.UpdateTable;

public sealed class UpdateTableCommandHandler : IRequestHandler<UpdateTableCommand>
{
    private readonly IRestaurantTableRepository _tableRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTableCommandHandler(
        IRestaurantTableRepository tableRepository,
        IUnitOfWork unitOfWork)
    {
        _tableRepository = tableRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpdateTableCommand request,
        CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByIdAsync(request.Id, cancellationToken);

        if (table is null)
            throw new KeyNotFoundException("Mesa não encontrada.");

        var tableAlreadyExists = await _tableRepository.ExistsByNumberAsync(
            request.Number,
            request.Id,
            cancellationToken);

        if (tableAlreadyExists)
            throw new InvalidOperationException("Já existe outra mesa com esse número.");

        table.UpdateNumber(request.Number);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}