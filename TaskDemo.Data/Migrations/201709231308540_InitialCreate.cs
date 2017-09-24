namespace TaskDemo.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Task",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Task", t => t.ParentId)
                .Index(t => t.ParentId)
                .Index(t => t.Name)
                .Index(t => new {t.ParentId, t.Name}, unique: true);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Task", "ParentId", "dbo.Task");
            DropIndex("dbo.Task", new[] { "ParentId" });
            DropIndex("dbo.Task", new[] { "Name" });
            DropIndex("dbo.Task", new[] { "ParentId", "Name" });
            DropTable("dbo.Task");
        }
    }
}
