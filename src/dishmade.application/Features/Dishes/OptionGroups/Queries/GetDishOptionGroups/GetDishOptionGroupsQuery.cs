using MediatR;

namespace dishmade.application.Features.Dishes.OptionGroups.Queries.GetDishOptionGroups;

public sealed record GetDishOptionGroupsQuery(Guid DishId)
    : IRequest<IReadOnlyList<DishOptionGroupResponse>>;