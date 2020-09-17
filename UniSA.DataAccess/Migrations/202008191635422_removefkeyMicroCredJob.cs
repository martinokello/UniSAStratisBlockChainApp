namespace UniSA.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removefkeyMicroCredJob : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MicroCredentials", "JobId", "dbo.Jobs");
            DropIndex("dbo.MicroCredentials", new[] { "JobId" });
            AddColumn("dbo.Jobs", "MicroCredentialId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "MicroCredentialId");
            CreateIndex("dbo.MicroCredentials", "JobId");
            AddForeignKey("dbo.MicroCredentials", "JobId", "dbo.Jobs", "JobId");
        }
    }
}
