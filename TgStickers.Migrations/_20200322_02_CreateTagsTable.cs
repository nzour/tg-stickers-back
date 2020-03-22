using FluentMigrator;

namespace TgStickers.Migrations
{
    [Migration(2020032202)]
    public class _20200322_02_CreateTagsTable : Migration
    {
        public override void Up()
        {
            Create.Table("Tags")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("Name").AsString().NotNullable();

            Create.UniqueConstraint("Tags_Name_key")
                .OnTable("Tags")
                .Column("Name");

            Create.Index("Tags_Name_idx")
                .OnTable("Tags")
                .OnColumn("Name");
        }

        public override void Down()
        {
            Delete.Table("Tags");
        }
    }
}