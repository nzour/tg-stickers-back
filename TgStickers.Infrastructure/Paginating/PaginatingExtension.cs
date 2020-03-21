using System.Linq;
using System.Threading.Tasks;
using NHibernate.Linq;

namespace TgStickers.Infrastructure.Paginating
{
    public static class PaginatingExtension
    {
        public static async Task<PaginatedData<T>> PaginateAsync<T>(this IQueryable<T> query, Pagination pagination)
        {
            var totalCount = (uint) await query.CountAsync();

            var data = await query
                .Take((int) pagination.Limit)
                .Skip((int) pagination.Offset)
                .ToListAsync();

            return new PaginatedData<T>(totalCount, data);
        }
    }
}