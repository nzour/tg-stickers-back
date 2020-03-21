using System.Collections.Generic;

namespace TgStickers.Infrastructure.Paginating
{
    public class PaginatedData<T>
    {
        public uint Total { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PaginatedData(uint total, IEnumerable<T> data)
        {
            Total = total;
            Data = data;
        }
    }
}