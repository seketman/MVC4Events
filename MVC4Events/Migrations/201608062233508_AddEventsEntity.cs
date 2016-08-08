namespace MVC4Events.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventsEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Technology = c.String(),
                        StartingDate = c.DateTime(nullable: false),
                        RegistrationLink = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Events");
        }
    }
}
