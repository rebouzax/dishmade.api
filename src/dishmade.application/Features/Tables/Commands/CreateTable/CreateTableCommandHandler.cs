using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.domain.Entities;
using MediatR;

namespace dishmade.application.Features.Tables.Commands.CreateTable;

public sealed class CreateTableCommandHandler : IRequestHandler<CreateTableCommand, Guid>
{
    private readonly IRestaurantTableRepository _tableRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTableCommandHandler(
        IRestaurantTableRepository tableRepository,
        IUnitOfWork unitOfWork)
    {
        _tableRepository = tableRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateTableCommand request,
        CancellationToken cancellationToken)
    {
        var tableAlreadyExists = await _tableRepository.ExistsByNumberAsync(
            request.Number,
            cancellationToken: cancellationToken);

        if (tableAlreadyExists)
            throw new InvalidOperationException("Já existe uma mesa com esse número.");

        var table = new RestaurantTable(request.Number);

        await _tableRepository.AddAsync(table, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return table.Id;
    }
}