using FluentNHibernate;
using FluentNHibernate.Mapping;
using TgStickers.Domain.Entity;

namespace TgStickers.Infrastructure.NHibernate.Mappings
{
    public class TagMap : ClassMap<Tag>
    {
        public TagMap()
        {
            Table("Stickers");

            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Name).Not.Nullable();

            HasManyToMany<StickerPack>(Reveal.Member<Tag>("Stickers"))
                .Access.CamelCaseField(Prefix.Underscore)
                .ExtraLazyLoad()
                .Cascade.Persist()
                .Inverse();
        }
    }
}