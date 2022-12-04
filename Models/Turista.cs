using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LocalWise.Models
{
    public class Turista
    {
        [Key]
        public int Id { get; set; }

        public int Tipo { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nome { get; set; }

        public string Foto { get; set; }

        [Required]
        public string Cpf { get; set; }

        [Required]
        [DataType(DataType.Date)]
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

        public string Hash { get; set; }
    }
}