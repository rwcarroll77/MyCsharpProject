namespace WebPresentation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedscreenNametoIdentityUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "screenName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "screenName");
        }
    }
}
