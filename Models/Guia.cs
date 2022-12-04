using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using static LocalWise.Models.Funcoes;

namespace LocalWise.Models
{
    public class Guia
    {
        [Key]
        public int Id { get; set; }

        public int Tipo { get; set; }

        public int Preco { get; set; }

        [Required]
        [Display(Name = "Cidade onde mora/atua como guia")]
        public string Cidade { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Nome completo")]
        public string Nome { get; set; }

        public string Foto { get; set; }

        [Required]
        public string Cpf { get; set; }

        [Required]
        [MinimumAge(18)]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string Nascimento { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Telefone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmarSenha { get; set; }

        public virtual ICollection<PontosGuias> PontosGuias { get; set; }

        public virtual ICollection<Pedido> Pedido { get; set; }

        public string Hash { get; set; }
    }
}