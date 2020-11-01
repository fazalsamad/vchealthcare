namespace VCHeathCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class treatments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Treatments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Treatments");
        }
    }
}
