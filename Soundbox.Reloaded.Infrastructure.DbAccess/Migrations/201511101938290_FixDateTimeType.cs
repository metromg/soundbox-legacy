namespace Soundbox.Reloaded.Infrastructure.DbAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDateTimeType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SoundCategory", "CreatedOn", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Sound", "CreatedOn", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sound", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SoundCategory", "CreatedOn", c => c.DateTime(nullable: false));
        }
    }
}
