using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Dishes.Options.Commands.DeleteDishOption;

public sealed class DeleteDishOptionCommandHandler
    : IRequestHandler<DeleteDishOptionCommand>
{
    private readonly IDishOptionRepository _optionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDishOptionCommandHandler(
        IDishOptionRepository optionRepository,
        IUnitOfWork unitOfWork)
    {
        _optionRepository = optionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeleteDishOptionCommand request,
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

        option.Delete();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}