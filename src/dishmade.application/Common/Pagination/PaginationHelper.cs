namespace dishmade.application.Common.Pagination;

public static class PaginationHelper
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    public static int NormalizePageNumber(int pageNumber)
    {
        return pageNumber <= 0 ? DefaultPageNumber : pageNumber;
    }

    public static int NormalizePageSize(int pageSize)
    {
        if (pageSize <= 0)
            return DefaultPageSize;

        return pageSize > MaxPageSize ? MaxPageSize : pageSize;
    }
}