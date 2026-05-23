using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Dishes.Commands.DeleteDish;

public sealed class DeleteDishCommandHandler : IRequestHandler<DeleteDishCommand>
{
    private readonly IDishRepository _dishRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDishCommandHandler(
        IDishRepository dishRepository,
        IUnitOfWork unitOfWork)
    {
        _dishRepository = dishRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeleteDishCommand request,
        CancellationToken cancellationToken)
    {
        var dish = await _dishRepository.GetByIdAsync(request.Id, cancellationToken);

        if (dish is null)
            throw new KeyNotFoundException("Prato não encontrado.");

        dish.Delete();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}