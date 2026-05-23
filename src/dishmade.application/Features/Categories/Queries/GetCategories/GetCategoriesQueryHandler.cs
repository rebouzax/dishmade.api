using dishmade.application.Abstractions.Repositories;
using dishmade.application.Common.Pagination;
using MediatR;

namespace dishmade.application.Features.Categories.Queries.GetCategories;

public sealed class GetCategoriesQueryHandler
    : IRequestHandler<GetCategoriesQuery, PagedResponse<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<PagedResponse<CategoryResponse>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var pageNumber = PaginationHelper.NormalizePageNumber(request.PageNumber);
        var pageSize = PaginationHelper.NormalizePageSize(request.PageSize);

        var result = await _categoryRepository.GetPagedAsync(
            request.Search,
            request.IsActive,
            pageNumber,
            pageSize,
            cancellationToken);

        var categories = result.Items
            .Select(category => new CategoryResponse(
                category.Id,
                category.Name,
                category.Description,
                category.IsActive,
                category.CreatedAt))
            .ToList();

        return new PagedResponse<CategoryResponse>(
            categories,
            pageNumber,
            pageSize,
            result.TotalCount);
    }
}