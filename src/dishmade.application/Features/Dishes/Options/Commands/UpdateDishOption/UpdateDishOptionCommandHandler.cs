using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Dishes.OptionGroups;
using MediatR;

namespace dishmade.application.Features.Dishes.Options.Commands.UpdateDishOption;

public sealed class UpdateDishOptionCommandHandler
    : IRequestHandler<UpdateDishOptionCommand, DishOptionResponse>
{
    private readonly IDishOptionRepository _optionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDishOptionCommandHandler(
        IDishOptionRepository optionRepository,
        IUnitOfWork unitOfWork)
    {
        _optionRepository = optionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DishOptionResponse> Handle(
        UpdateDishOptionCommand request,
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

        option.Update(
            request.Name,
            request.AdditionalPrice);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DishOptionResponse(
            option.Id,
            option.OptionGroupId,
            option.Name,
            option.AdditionalPrice,
            option.IsAvailable);
    }
}