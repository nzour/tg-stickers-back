using FluentNHibernate.Mapping;
using TgStickers.Domain.Entity;

namespace TgStickers.Infrastructure.NHibernate.Mappings
{
    public class TagMap : ClassMap<Tag>
    {
        public TagMap()
        {
            Table("Tags");

            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Name).Not.Nullable();
        }
    }
}