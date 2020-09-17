namespace UniSA.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class linkMoocToMicroCredential : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MicroCredentials", "MoocProviderId", c => c.Int(nullable: false));
            CreateIndex("dbo.MicroCredentials", "MoocProviderId");
            AddForeignKey("dbo.MicroCredentials", "MoocProviderId", "dbo.MoocProviders", "MoocProviderId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MicroCredentials", "MoocProviderId", "dbo.MoocProviders");
            DropIndex("dbo.MicroCredentials", new[] { "MoocProviderId" });
            DropColumn("dbo.MicroCredentials", "MoocProviderId");
        }
    }
}
