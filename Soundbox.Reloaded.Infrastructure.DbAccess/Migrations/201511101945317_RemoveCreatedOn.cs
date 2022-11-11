namespace Soundbox.Reloaded.Infrastructure.DbAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCreatedOn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SoundCategory", "CreatedOn");
            DropColumn("dbo.Sound", "CreatedOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sound", "CreatedOn", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.SoundCategory", "CreatedOn", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
