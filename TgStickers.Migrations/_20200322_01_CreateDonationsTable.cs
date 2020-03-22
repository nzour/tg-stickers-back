using FluentMigrator;

namespace TgStickers.Migrations
{
    [Migration(2020032201)]
    public class _20200322_01_CreateDonationsTable : Migration
    {
        public override void Up()
        {
            Create.Table("Donations")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("SponsorName").AsString().Nullable()
                .WithColumn("SponsorEmail").AsString().Nullable()
                .WithColumn("Message").AsString().Nullable()
                .WithColumn("Money").AsInt32().NotNullable()
                .WithColumn("Currency").AsString().NotNullable()
                .WithColumn("CreatedAt").AsDateTime().NotNullable()
                .WithColumn("StickerPackId").AsGuid().NotNullable();

            Create.Index("Donations_StickerPackId_idx")
                .OnTable("Donations")
                .OnColumn("StickerPackId");

            Create.ForeignKey("Donations_StickerPackId_fkey")
                .FromTable("Donations")
                .ForeignColumn("StickerPackid")
                .ToTable("StickerPacks")
                .PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("Donations");
        }
    }
}