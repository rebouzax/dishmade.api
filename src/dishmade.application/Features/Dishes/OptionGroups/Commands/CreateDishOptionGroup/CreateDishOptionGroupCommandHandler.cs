using dishmade.application.Abstractions.Auth;
using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.domain.Entities;
using MediatR;

namespace dishmade.application.Features.Dishes.OptionGroups.Commands.CreateDishOptionGroup;

public sealed class CreateDishOptionGroupCommandHandler
    : IRequestHandler<CreateDishOptionGroupCommand, DishOptionGroupResponse>
{
    private readonly IDishRepository _dishRepository;
    private readonly IDishOptionGroupRepository _groupRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDishOptionGroupCommandHandler(
        IDishRepository dishRepository,
        IDishOptionGroupRepository groupRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _dishRepository = dishRepository;
        _groupRepository = groupRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<DishOptionGroupResponse> Handle(
        CreateDishOptionGroupCommand request,
        CancellationToken cancellationToken)
    {
        var restaurantId = _currentUserService.GetRequiredRestaurantId();

        var dish = await _dishRepository.GetByIdAsync(
            request.DishId,
            cancellationToken);

        if (dish is null)
            throw new KeyNotFoundException("Prato não encontrado.");

        var group = new DishOptionGroup(
            request.DishId,
            restaurantId,
            request.Name,
            request.IsRequired,
            request.MinSelection,
            request.MaxSelection);

        await _groupRepository.AddAsync(group, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DishOptionGroupResponse(
            group.Id,
            group.DishId,
            group.Name,
            group.IsRequired,
            group.MinSelection,
            group.MaxSelection,
            group.IsActive,
            []);
    }
}