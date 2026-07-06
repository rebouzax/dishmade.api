using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Dishes.OptionGroups;
using MediatR;

namespace dishmade.application.Features.Dishes.Options.Commands.SetDishOptionAvailability;

public sealed class SetDishOptionAvailabilityCommandHandler
    : IRequestHandler<SetDishOptionAvailabilityCommand, DishOptionResponse>
{
    private readonly IDishOptionRepository _optionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetDishOptionAvailabilityCommandHandler(
        IDishOptionRepository optionRepository,
        IUnitOfWork unitOfWork)
    {
        _optionRepository = optionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DishOptionResponse> Handle(
        SetDishOptionAvailabilityCommand request,
        CancellationToken cancellationToken)
    {
        var option = await _optionRepository.GetByIdAsync(
            request.OptionId,
            cancellationToken);

        if (option is null ||
            option.OptionGroupId != request.OptionGroupId ||
            option.OptionGroup.DishId != request.DishId)
        {
            throw new KeyNotFoundException("Opção não encontrada.");
        }

        if (request.IsAvailable)
            option.SetAvailable();
        else
            option.SetUnavailable();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DishOptionResponse(
            option.Id,
            option.OptionGroupId,
            option.Name,
            option.AdditionalPrice,
            option.IsAvailable);
    }
}