namespace Soundbox.Reloaded.Infrastructure.DbAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddSound : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sound",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 64),
                        SoundFileName = c.String(nullable: false, maxLength: 256),
                        ImageFileName = c.String(nullable: false, maxLength: 256),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.Sound");
        }
    }
}
