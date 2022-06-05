namespace ScopoERP.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class poEmployeeMapping : DbMigration
    {
        public override void Up()
        {
           
            
            CreateTable(
                "dbo.POEmployeeMappings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        ProductionPlanningID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            
        }
        
        public override void Down()
        {
        }
    }
}
