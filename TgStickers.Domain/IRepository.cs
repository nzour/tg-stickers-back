using System.Linq;
using System.Threading.Tasks;

namespace TgStickers.Domain
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> FindByIdAsync(object id);

        Task<IQueryable<TEntity>> FindAllAsync();

        Task SaveAsync(TEntity entity);
    }
}