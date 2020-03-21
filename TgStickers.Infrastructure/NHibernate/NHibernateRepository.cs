using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using TgStickers.Domain;

namespace TgStickers.Infrastructure.NHibernate
{
    public class NHibernateRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ISession _session;

        public NHibernateRepository(ISession session)
        {
            _session = session;
        }

        public async Task<TEntity?> FindByIdAsync(object id)
        {
            return await _session.GetAsync<TEntity>(id);
        }

        public IQueryable<TEntity> FindAll()
        {
            return _session.Query<TEntity>();
        }

        public async Task SaveAsync(TEntity entity)
        {
            await _session.SaveAsync(entity);
        }
    }
}