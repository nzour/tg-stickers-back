using System.Data;
using FluentMigrator;

namespace TgStickers.Migrations
{
    [Migration(2020032203)]
    public class _20200322_03_CreateStickerPackTagPivotTable : Migration
    {
        public override void Up()
        {
            Create.Table("StickerTags_Pivot")
                .WithColumn("StickerPackId").AsGuid().NotNullable()
                .WithColumn("TagId").AsGuid().NotNullable();

            Create.PrimaryKey()
                .OnTable("StickerTags_Pivot")
                .Columns("StickerPackId", "TagId");

            Create.ForeignKey("StickerTags_Pivot_StickerPackId_fkey")
                .FromTable("StickerTags_Pivot")
                .ForeignColumn("StickerPackId")
                .ToTable("StickerPacks")
                .PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);

            Create.ForeignKey("StickerTags_Pivot_TagId_fkey")
                .FromTable("StickerTags_Pivot")
                .ForeignColumn("TagId")
                .ToTable("Tags")
                .PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.Table("StickerTags_Pivot");
        }
    }
}