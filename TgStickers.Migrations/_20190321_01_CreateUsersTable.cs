using FluentMigrator;

namespace TgStickers.Migrations
{
    [Migration(2019032101)]
    public class _20190321_01_CreateUsersTable : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Login").AsString().NotNullable()
                .WithColumn("Password").AsString().NotNullable()
                .WithColumn("Role").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Users");
        }
    }
}