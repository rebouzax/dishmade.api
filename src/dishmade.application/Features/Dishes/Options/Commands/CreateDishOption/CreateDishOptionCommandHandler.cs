using dishmade.application.Abstractions.Auth;
using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Dishes.OptionGroups;
using dishmade.domain.Entities;
using MediatR;

namespace dishmade.application.Features.Dishes.Options.Commands.CreateDishOption;

public sealed class CreateDishOptionCommandHandler
    : IRequestHandler<CreateDishOptionCommand, DishOptionResponse>
{
    private readonly IDishOptionGroupRepository _groupRepository;
    private readonly IDishOptionRepository _optionRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDishOptionCommandHandler(
        IDishOptionGroupRepository groupRepository,
        IDishOptionRepository optionRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _optionRepository = optionRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<DishOptionResponse> Handle(
        CreateDishOptionCommand request,
        CancellationToken cancellationToken)
    {
        var restaurantId = _currentUserService.GetRequiredRestaurantId();

        var group = await _groupRepository.GetByIdAsync(
            request.OptionGroupId,
            cancellationToken);

        if (group is null || group.DishId != request.DishId)
            throw new KeyNotFoundException("Grupo de opções não encontrado.");

        var option = new DishOption(
            request.OptionGroupId,
            restaurantId,
            request.Name,
            request.AdditionalPrice);

        await _optionRepository.AddAsync(option, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DishOptionResponse(
            option.Id,
            option.OptionGroupId,
            option.Name,
            option.AdditionalPrice,
            option.IsAvailable);
    }
}