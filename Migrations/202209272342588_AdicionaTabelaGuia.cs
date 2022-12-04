namespace LocalWise.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionaTabelaGuia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.gui_guia",
                c => new
                    {
                        gui_codigo = c.Int(nullable: false, identity: true),
                        gui_nome = c.String(nullable: false, maxLength: 200),
                        gui_cpf = c.String(nullable: false),
                        gui_nascimento = c.String(nullable: false),
                        gui_telefone = c.String(nullable: false),
                        gui_email = c.String(nullable: false),
                        gui_senha = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.gui_codigo);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.gui_guia");
        }
    }
}
