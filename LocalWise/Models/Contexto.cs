using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace LocalWise.Models
{
    public class Contexto : DbContext
    {
        public Contexto() : base(nameOrConnectionString: "StringConexao") { }

        public DbSet<Guia> Guia { get; set; }
        public DbSet<Turista> Turista { get; set; }

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            var gui = mb.Entity<Guia>();
            gui.ToTable("gui_guia");
            gui.Property(x => x.Id).HasColumnName("gui_codigo");
            gui.Property(x => x.Nome).HasColumnName("gui_nome");//.HasMaxLength(200);
            gui.Property(x => x.Cpf).HasColumnName("gui_cpf");
            gui.Property(x => x.Nascimento).HasColumnName("gui_nascimento");
            gui.Property(x => x.Telefone).HasColumnName("gui_telefone");
            gui.Property(x => x.Email).HasColumnName("gui_email");
            gui.Property(x => x.Senha).HasColumnName("gui_senha");

            var tur = mb.Entity<Turista>();
            tur.ToTable("tur_turista");
            tur.Property(x => x.Id).HasColumnName("tur_codigo");
            tur.Property(x => x.Nome).HasColumnName("tur_nome");//.HasMaxLength(200);
            tur.Property(x => x.Cpf).HasColumnName("tur_cpf");
            tur.Property(x => x.Nascimento).HasColumnName("tur_nascimento");
            tur.Property(x => x.Telefone).HasColumnName("tur_telefone");
            tur.Property(x => x.Email).HasColumnName("tur_email");
            tur.Property(x => x.Senha).HasColumnName("tur_senha");
        }
    }
}