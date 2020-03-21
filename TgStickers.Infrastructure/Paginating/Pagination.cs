namespace TgStickers.Infrastructure.Paginating
{
    public class Pagination
    {
        public uint Limit { get; set; } = 100;
        public uint Offset { get; set; } = 0;
    }
}