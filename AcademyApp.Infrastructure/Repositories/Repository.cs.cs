using AcademyApp.Domain.Interfaces;
using AcademyApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace AcademyApp.Infrastructure.Repositories
{
    public class Repository<T>(AcademyDbContext ctx) : IRepository<T> where T : class
    {
        public async Task<T?> GetByIdAsync(int id) => await ctx.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> ListAsync(
            int page, int pageSize, string? sortBy = null, bool desc = false,
            Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> q = ctx.Set<T>();
            if (filter is not null) q = q.Where(filter);

            if (!string.IsNullOrWhiteSpace(sortBy))
                q = desc ? q.OrderByDescending(e => EF.Property<object>(e, sortBy))
                         : q.OrderBy(e => EF.Property<object>(e, sortBy));

            return await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> q = ctx.Set<T>();
            if (filter is not null) q = q.Where(filter);
            return q.CountAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            ctx.Set<T>().Add(entity);
            await ctx.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            ctx.Set<T>().Update(entity);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            ctx.Set<T>().Remove(entity);
            await ctx.SaveChangesAsync();
        }
    }
}
