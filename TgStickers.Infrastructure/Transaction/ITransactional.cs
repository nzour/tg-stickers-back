using System;
using System.Threading.Tasks;

namespace TgStickers.Infrastructure.Transaction
{
    public interface ITransactional
    {
        Task ExecuteAsync(Action action);

        Task ExecuteAsync(Func<Task> action);

        Task<T> ExecuteAsync<T>(Func<T> action);
        
        Task<T> ExecuteAsync<T>(Func<Task<T>> action);
    }
}
