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

        public DbSet<PontosTuristicos> PontosTuristicos { get; set; }

        public DbSet<PontosGuias> PontosGuias { get; set; }

        public DbSet<Pedido> Pedido { get; set; }

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            var gui = mb.Entity<Guia>();
            gui.ToTable("gui_guia");
            gui.Property(x => x.Id).HasColumnName("gui_codigo");
            gui.Property(x => x.Tipo).HasColumnName("gui_tipo");
            gui.Property(x => x.Preco).HasColumnName("gui_preco");
            gui.Property(x => x.Cidade).HasColumnName("gui_cidade");
            gui.Property(x => x.Nome).HasColumnName("gui_nome");//.HasMaxLength(200);
            gui.Property(x => x.Foto).HasColumnName("gui_foto");
            gui.Property(x => x.Cpf).HasColumnName("gui_cpf");
            gui.Property(x => x.Nascimento).HasColumnName("gui_nascimento");
            gui.Property(x => x.Telefone).HasColumnName("gui_telefone");
            gui.Property(x => x.Email).HasColumnName("gui_email");
            gui.Property(x => x.Senha).HasColumnName("gui_senha");
            gui.Property(x => x.ConfirmarSenha).HasColumnName("gui_confirmasenha");

            var tur = mb.Entity<Turista>();
            tur.ToTable("tur_turista");
            tur.Property(x => x.Id).HasColumnName("tur_codigo");
            tur.Property(x => x.Tipo).HasColumnName("tur_tipo");
            tur.Property(x => x.Nome).HasColumnName("tur_nome");//.HasMaxLength(200);
            tur.Property(x => x.Foto).HasColumnName("tur_foto");
            tur.Property(x => x.Cpf).HasColumnName("tur_cpf");
            tur.Property(x => x.Nascimento).HasColumnName("tur_nascimento");
            tur.Property(x => x.Telefone).HasColumnName("tur_telefone");
            tur.Property(x => x.Email).HasColumnName("tur_email");
            tur.Property(x => x.Senha).HasColumnName("tur_senha");
            tur.Property(x => x.Hash).HasColumnName("tur_hash");

            var pon = mb.Entity<PontosTuristicos>();
            pon.ToTable("pon_ponto");
            pon.Property(x => x.Id).HasColumnName("pon_id");
            pon.Property(x => x.Nome).HasColumnName("pon_nome");
            pon.Property(x => x.Foto).HasColumnName("pon_foto");
            pon.Property(x => x.Descricao).HasColumnName("pon_descricao");
            pon.Property(x => x.Tipo).HasColumnName("pon_tipo");

            var ptg = mb.Entity<PontosGuias>();
            ptg.ToTable("ptg_pontoguia");
            ptg.Property(x => x.Id).HasColumnName("ptg_id");
            ptg.Property(x => x.GuiaId).HasColumnName("ptg_guiaid");
            ptg.Property(x => x.PontosTuristicosId).HasColumnName("ptg_pontosid");

            var pdd = mb.Entity<Pedido>();
            pdd.ToTable("pdd_pedido");
            pdd.Property(x => x.Id).HasColumnName("pdd_id");
            pdd.Property(x => x.Status).HasColumnName("pdd_status");
            pdd.Property(x => x.Cidade).HasColumnName("pdd_cidade");
            pdd.Property(x => x.QtdPessoas).HasColumnName("pdd_qtdpessoas");
            pdd.Property(x => x.Locomocao).HasColumnName("pdd_locomocao");
            pdd.Property(x => x.Tipo).HasColumnName("pdd_tipo");
            pdd.Property(x => x.Data).HasColumnName("pdd_data");
            pdd.Property(x => x.GuiaId).HasColumnName("ptg_guiaid");
            pdd.Property(x => x.TuristaId).HasColumnName("ptg_turistaid");
        }
    }
}