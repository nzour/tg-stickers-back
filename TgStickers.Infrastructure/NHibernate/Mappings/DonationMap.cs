using FluentNHibernate.Mapping;
using NHibernate.Type;
using TgStickers.Domain;
using TgStickers.Domain.Entity;

namespace TgStickers.Infrastructure.NHibernate.Mappings
{
    public class DonationMap : ClassMap<Donation>
    {
        public DonationMap()
        {
            Table("Donations");

            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.SponsorName).Nullable();
            Map(x => x.SponsorEmail).Nullable();
            Map(x => x.Message).Nullable();
            Map(x => x.Money).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();

            Map(x => x.Currency)
                .CustomType<EnumStringType<Currency>>()
                .Not.Nullable();

            References(x => x.StickerPack, "StickerPackId")
                .Not.Nullable()
                .Cascade.Persist()
                .Cascade.Delete();
        }
    }
}