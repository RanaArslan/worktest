
namespace ContosoUniversity.Migrations

{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AlterEntryCoulmn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Person", "EntryDate", c => c.DateTime(nullable: true));
        }
        public override void Down()
        {
            DropColumn("dbo.Person", "EntryDate");
        }
    }
}