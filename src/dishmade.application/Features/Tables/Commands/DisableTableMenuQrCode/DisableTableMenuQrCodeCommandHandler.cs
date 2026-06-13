using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Tables.Commands.DisableTableMenuQrCode;

public sealed class DisableTableMenuQrCodeCommandHandler
    : IRequestHandler<DisableTableMenuQrCodeCommand>
{
    private readonly IRestaurantTableRepository _tableRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DisableTableMenuQrCodeCommandHandler(
        IRestaurantTableRepository tableRepository,
        IUnitOfWork unitOfWork)
    {
        _tableRepository = tableRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DisableTableMenuQrCodeCommand request,
        CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByIdAsync(
            request.TableId,
            cancellationToken);

        if (table is null)
            throw new KeyNotFoundException("Mesa não encontrada.");

        table.DisableMenuQrCode();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}