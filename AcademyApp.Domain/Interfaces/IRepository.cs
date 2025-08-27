using System.Linq.Expressions;


namespace AcademyApp.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAsync(
            int page, int pageSize,
            string? sortBy = null, bool desc = false,
            Expression<Func<T, bool>>? filter = null);
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
