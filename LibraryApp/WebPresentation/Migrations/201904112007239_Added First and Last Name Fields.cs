namespace WebPresentation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFirstandLastNameFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "firstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "lastName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "lastName");
            DropColumn("dbo.AspNetUsers", "firstName");
        }
    }
}
