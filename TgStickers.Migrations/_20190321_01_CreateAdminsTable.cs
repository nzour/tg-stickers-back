using FluentMigrator;

namespace TgStickers.Migrations
{
    [Migration(2019032101)]
    public class _20190321_01_CreateAdminsTable : Migration
    {
        public override void Up()
        {
            Create.Table("Admins")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Login").AsString().NotNullable()
                .WithColumn("Password").AsString().NotNullable()
                .WithColumn("CreatedAt").AsDateTime().NotNullable();

            Create.UniqueConstraint("Admins_Login_key")
                .OnTable("Admins")
                .Column("Login");

            Create.Index("Admins_Login_ids")
                .OnTable("Admins")
                .OnColumn("Login");
        }

        public override void Down()
        {
            Delete.Table("Admins");
        }
    }
}