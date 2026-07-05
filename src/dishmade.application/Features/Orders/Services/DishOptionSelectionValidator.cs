using dishmade.domain.Entities;

namespace dishmade.application.Features.Orders.Services;

public static class DishOptionSelectionValidator
{
    public static void Validate(
        IReadOnlyList<DishOptionGroup> groups,
        IReadOnlyList<DishOption> selectedOptions)
    {
        foreach (var group in groups.Where(group => group.IsActive && !group.IsDeleted))
        {
            var selectedCount = selectedOptions.Count(option =>
                option.OptionGroupId == group.Id);

            if (group.IsRequired && selectedCount < group.MinSelection)
            {
                throw new InvalidOperationException(
                    $"O grupo '{group.Name}' exige pelo menos {group.MinSelection} opção(ões).");
            }

            if (selectedCount < group.MinSelection)
            {
                throw new InvalidOperationException(
                    $"O grupo '{group.Name}' exige no mínimo {group.MinSelection} opção(ões).");
            }

            if (selectedCount > group.MaxSelection)
            {
                throw new InvalidOperationException(
                    $"O grupo '{group.Name}' permite no máximo {group.MaxSelection} opção(ões).");
            }
        }
    }
}