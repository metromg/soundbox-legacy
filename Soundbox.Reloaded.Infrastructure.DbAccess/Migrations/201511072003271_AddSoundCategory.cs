namespace Soundbox.Reloaded.Infrastructure.DbAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddSoundCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SoundCategory",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 64),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Sound", "CategoryId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Sound", "CategoryId");
            AddForeignKey("dbo.Sound", "CategoryId", "dbo.SoundCategory", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sound", "CategoryId", "dbo.SoundCategory");
            DropIndex("dbo.Sound", new[] { "CategoryId" });
            DropColumn("dbo.Sound", "CategoryId");
            DropTable("dbo.SoundCategory");
        }
    }
}
