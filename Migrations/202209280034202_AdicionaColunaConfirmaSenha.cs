namespace LocalWise.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionaColunaConfirmaSenha : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.gui_guia", "ConfirmarSenha", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.gui_guia", "ConfirmarSenha");
        }
    }
}
