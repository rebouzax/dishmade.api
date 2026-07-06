using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Dishes.OptionGroups.Commands.DeleteDishOptionGroup;

public sealed class DeleteDishOptionGroupCommandHandler
    : IRequestHandler<DeleteDishOptionGroupCommand>
{
    private readonly IDishRepository _dishRepository;
    private readonly IDishOptionGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDishOptionGroupCommandHandler(
        IDishRepository dishRepository,
        IDishOptionGroupRepository groupRepository,
        IUnitOfWork unitOfWork)
    {
        _dishRepository = dishRepository;
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeleteDishOptionGroupCommand request,
        CancellationToken cancellationToken)
    {
        var dish = await _dishRepository.GetByIdAsync(
            request.DishId,
            cancellationToken);

        if (dish is null)
            throw new KeyNotFoundException("Prato não encontrado.");

        var group = await _groupRepository.GetByIdAsync(
            request.OptionGroupId,
            cancellationToken);

        if (group is null || group.DishId != request.DishId)
            throw new KeyNotFoundException("Grupo de opções não encontrado.");

        group.Delete();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}