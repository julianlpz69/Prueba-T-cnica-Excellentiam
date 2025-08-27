

namespace AcademyApp.Application.Pagination
{
    public record PagedResult<T>(IReadOnlyList<T> Items, int Total, int Page, int PageSize);

}
