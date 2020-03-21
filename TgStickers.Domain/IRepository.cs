using System.Linq;
using System.Threading.Tasks;

namespace TgStickers.Domain
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> FindByIdAsync(object id);

        IQueryable<TEntity> FindAll();

        Task SaveAsync(TEntity entity);
    }
}