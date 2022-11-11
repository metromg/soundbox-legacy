namespace Soundbox.Reloaded.Infrastructure.DbAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ImproveSound : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sound", "ArtistName", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.Sound", "ImageFileName", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sound", "ImageFileName", c => c.String(nullable: false, maxLength: 256));
            DropColumn("dbo.Sound", "ArtistName");
        }
    }
}
