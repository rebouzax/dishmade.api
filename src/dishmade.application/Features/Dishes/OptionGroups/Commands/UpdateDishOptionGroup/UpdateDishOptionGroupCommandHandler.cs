using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Dishes.OptionGroups.Commands.UpdateDishOptionGroup;

public sealed class UpdateDishOptionGroupCommandHandler
    : IRequestHandler<UpdateDishOptionGroupCommand, DishOptionGroupResponse>
{
    private readonly IDishRepository _dishRepository;
    private readonly IDishOptionGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDishOptionGroupCommandHandler(
        IDishRepository dishRepository,
        IDishOptionGroupRepository groupRepository,
        IUnitOfWork unitOfWork)
    {
        _dishRepository = dishRepository;
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DishOptionGroupResponse> Handle(
        UpdateDishOptionGroupCommand request,
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

        group.Update(
            request.Name,
            request.IsRequired,
            request.MinSelection,
            request.MaxSelection);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DishOptionGroupResponse(
            group.Id,
            group.DishId,
            group.Name,
            group.IsRequired,
            group.MinSelection,
            group.MaxSelection,
            group.IsActive,
            group.Options
                .Where(option => !option.IsDeleted)
                .Select(option => new DishOptionResponse(
                    option.Id,
                    option.OptionGroupId,
                    option.Name,
                    option.AdditionalPrice,
                    option.IsAvailable))
                .ToList());
    }
}