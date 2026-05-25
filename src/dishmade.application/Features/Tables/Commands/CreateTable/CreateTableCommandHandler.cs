using dishmade.application.Abstractions.Auth;
using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.domain.Entities;
using MediatR;

namespace dishmade.application.Features.Tables.Commands.CreateTable;

public sealed class CreateTableCommandHandler : IRequestHandler<CreateTableCommand, Guid>
{
    private readonly IRestaurantTableRepository _tableRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CreateTableCommandHandler(
        IRestaurantTableRepository tableRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _tableRepository = tableRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
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

        var restaurantId = _currentUserService.GetRequiredRestaurantId();

        var table = new RestaurantTable(
            request.Number,
            restaurantId);

        await _tableRepository.AddAsync(table, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return table.Id;
    }
}