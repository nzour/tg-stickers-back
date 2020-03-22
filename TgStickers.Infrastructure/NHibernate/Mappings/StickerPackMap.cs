using FluentNHibernate;
using FluentNHibernate.Mapping;
using TgStickers.Domain.Entity;

namespace TgStickers.Infrastructure.NHibernate.Mappings
{
    public class StickerPackMap : ClassMap<StickerPack>
    {
        public StickerPackMap()
        {
            Table("StickerPacks");

            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Name).Not.Nullable();
            Map(x => x.SharedUrl).Not.Nullable();
            Map(x => x.Claps).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();

            References(x => x.CreatedBy, "CreatedById")
                .Not.Nullable()
                .Cascade.Persist()
                .Cascade.Delete()
                .LazyLoad();

            HasMany<Donation>(Reveal.Member<StickerPack>("Donations"))
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .ExtraLazyLoad()
                .Inverse();

            HasManyToMany<StickerPack>(Reveal.Member<StickerPack>("Tags"))
                .Access.CamelCaseField(Prefix.Underscore)
                .ExtraLazyLoad()
                .ParentKeyColumn("StickerPackId")
                .ChildKeyColumn("TagId")
                .Table("StickerTags_Pivot");
        }
    }
}