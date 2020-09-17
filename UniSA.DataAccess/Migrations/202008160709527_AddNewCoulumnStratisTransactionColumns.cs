namespace UniSA.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewCoulumnStratisTransactionColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CandidateMicroCredentialCourses", "transactionHex", c => c.String());
            AddColumn("dbo.CandidateMicroCredentialCourses", "transactionId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CandidateMicroCredentialCourses", "transactionId");
            DropColumn("dbo.CandidateMicroCredentialCourses", "transactionHex");
        }
    }
}
