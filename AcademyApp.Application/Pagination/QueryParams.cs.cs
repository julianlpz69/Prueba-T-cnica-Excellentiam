

namespace AcademyApp.Application.Pagination
{
    public record QueryParams(int Page = 1, int PageSize = 10, string? SortBy = null, bool Desc = false, string? Search = null);

}
