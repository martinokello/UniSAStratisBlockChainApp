namespace UniSA.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class includeMicCredBadgeId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CandidateMicroCredentialCourses", "MicroCredentialBadgeId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CandidateMicroCredentialCourses", "MicroCredentialBadgeId");
        }
    }
}
