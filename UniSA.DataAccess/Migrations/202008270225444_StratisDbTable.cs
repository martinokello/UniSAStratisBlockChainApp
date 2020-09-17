namespace UniSA.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StratisDbTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StratisAccounts",
                c => new
                    {
                        StratisAccountId = c.Int(nullable: false, identity: true),
                        AccountName = c.String(),
                        AccountWalletName = c.String(),
                        EmailAddress = c.String(),
                        AccountStratisAddress1 = c.String(),
                        AccountStratisAddress2 = c.String(),
                        AccountStratisAddress3 = c.String(),
                    })
                .PrimaryKey(t => t.StratisAccountId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StratisAccounts");
        }
    }
}
