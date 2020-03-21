using FluentNHibernate.Mapping;
using NHibernate.Type;
using TgStickers.Domain.Entity;
using TgStickers.Domain.Enums;

namespace TgStickers.Infrastructure.NHibernate.Mappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("Users");

            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Name).Not.Nullable();
            Map(x => x.Login).Not.Nullable();
            Map(x => x.Password).Not.Nullable();

            Map(x => x.Role)
                .CustomType<EnumStringType<Role>>()
                .Not.Nullable();
        }
    }
}