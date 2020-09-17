namespace UniSA.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsWalletLoadedColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StratisAccounts", "IsWalletLoaded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StratisAccounts", "IsWalletLoaded");
        }
    }
}
