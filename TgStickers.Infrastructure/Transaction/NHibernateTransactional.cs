using System;
using System.Threading.Tasks;
using NHibernate;

namespace TgStickers.Infrastructure.Transaction
{
    public class NHibernateTransactional : ITransactional
    {
        private readonly ISession _session;

        public NHibernateTransactional(ISession session)
        {
            _session = session;
        }

        public async Task DoAsync(Action action)
        {
            using var transaction = _session.BeginTransaction();

            try
            {
                action();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DoAsync(Func<Task> action)
        {
            using var transaction = _session.BeginTransaction();

            try
            {
                await action();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<T> DoAsync<T>(Func<T> action)
        {
            using var transaction = _session.BeginTransaction();

            try
            {
                var result = action();
                await transaction.CommitAsync();

                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<T> DoAsync<T>(Func<Task<T>> action)
        {
            using var transaction = _session.BeginTransaction();

            try
            {
                var result = await action();
                await transaction.CommitAsync();

                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
