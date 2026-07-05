using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Dishes.OptionGroups.Queries.GetDishOptionGroups;

public sealed class GetDishOptionGroupsQueryHandler
    : IRequestHandler<GetDishOptionGroupsQuery, IReadOnlyList<DishOptionGroupResponse>>
{
    private readonly IDishRepository _dishRepository;
    private readonly IDishOptionGroupRepository _groupRepository;

    public GetDishOptionGroupsQueryHandler(
        IDishRepository dishRepository,
        IDishOptionGroupRepository groupRepository)
    {
        _dishRepository = dishRepository;
        _groupRepository = groupRepository;
    }

    public async Task<IReadOnlyList<DishOptionGroupResponse>> Handle(
        GetDishOptionGroupsQuery request,
        CancellationToken cancellationToken)
    {
        var dish = await _dishRepository.GetByIdAsync(
            request.DishId,
            cancellationToken);

        if (dish is null)
            throw new KeyNotFoundException("Prato não encontrado.");

        var groups = await _groupRepository.GetByDishIdAsync(
            request.DishId,
            cancellationToken);

        return groups
            .Select(group => new DishOptionGroupResponse(
                group.Id,
                group.DishId,
                group.Name,
                group.IsRequired,
                group.MinSelection,
                group.MaxSelection,
                group.IsActive,
                group.Options
                    .Select(option => new DishOptionResponse(
                        option.Id,
                        option.OptionGroupId,
                        option.Name,
                        option.AdditionalPrice,
                        option.IsAvailable))
                    .ToList()))
            .ToList();
    }
}