namespace Component.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateOrUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sys_LocaleResource",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50),
                        Language = c.String(maxLength: 50),
                        ResourceModule = c.String(maxLength: 1024),
                        ResourceCode = c.String(maxLength: 1024),
                        ResourceValue = c.String(),
                        LocaleResourceType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Sys_LocaleResource");
        }
    }
}
