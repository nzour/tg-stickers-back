using System;
using System.Threading.Tasks;

namespace TgStickers.Infrastructure.Transaction
{
    public interface ITransactional
    {
        Task DoAsync(Action action);

        Task DoAsync(Func<Task> action);

        Task<T> DoAsync<T>(Func<T> action);
        
        Task<T> DoAsync<T>(Func<Task<T>> action);
    }
}
