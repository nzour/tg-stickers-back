using FluentMigrator;
using static System.Data.Rule;

namespace TgStickers.Migrations
{
    [Migration(2020032102)]
    public class _20200321_02_CreateStickerPacksTable : Migration
    {
        public override void Up()
        {
            Create.Table("StickerPacks")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Alias").AsString().NotNullable()
                .WithColumn("CreatedAt").AsDateTime().NotNullable()
                .WithColumn("CreatedById").AsGuid().NotNullable();

            Create.Index("StickerPacks_Name_idx")
                .OnTable("StickerPacks")
                .OnColumn("Name");

            Create.ForeignKey("StickerPacks_CreatedById_fkey")
                .FromTable("StickerPacks")
                .ForeignColumn("CreatedById")
                .ToTable("Admins")
                .PrimaryColumn("Id")
                .OnDelete(Cascade);
        }

        public override void Down()
        {
            Delete.Table("StickerPacks");
        }
    }
}