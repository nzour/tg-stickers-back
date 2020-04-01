using FluentMigrator;

namespace TgStickers.Migrations
{
    [Migration(2020040101)]
    public class _20200401_01_AddColumnClaps_ToStickerPacksTable : Migration
    {
        public override void Up()
        {
            Alter.Table("StickerPacks")
                .AddColumn("Claps").AsInt32().NotNullable().WithDefaultValue(0);
        }

        public override void Down()
        {
            Delete.Column("Claps").FromTable("StickerPacks");
        }
    }
}
