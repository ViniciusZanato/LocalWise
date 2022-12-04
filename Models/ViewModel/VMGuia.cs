using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using static LocalWise.Models.Funcoes;

namespace LocalWise.Models
{
    public class CadastroGuia
    {
        [Required]
        [Display(Name = "Cidade onde mora/atua como guia")]
        public string Cidade { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Nome completo")]
        public string Nome { get; set; }

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
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,12})", ErrorMessage = "A senha deve conter ao menos uma letra maiúscula, minúscula e um número. Deve conter no mínimo 6 caracteres.")]
        public string Senha { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Senha")]
        [Display(Name = "Confirma Senha")]
        public string ConfirmaSenha { get; set; }
    }
    public class AcessoGuia
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,12})", ErrorMessage = "A senha deve conter ao menos uma letra maiúscula, minúscula e um número. Deve conter no mínimo 6 caracteres.")]
        public string Senha { get; set; }
    }

    public class CadastroPonto
    {
        [Required]
        public string Foto { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Descrição")]
        [MaxLength(200)]
        public string Descricao { get; set; }
    }

    public class MensagemGuia
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Assunto { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Mensagem")]
        public string CorpoMsg { get; set; }
    }
    public class EsqueceuSenhaGuia
    {
        [EmailAddress]
        [Required]
        [Display(Name = "Digite o e-mail vinculado a sua conta")]
        public string Email { get; set; }
    }
    public class RedefinirSenhaGuia
    {
        public string Email { get; set; }
        public string Hash { get; set; }
        [DataType(DataType.Password)]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,12})", ErrorMessage = "A senha deve conter aos menos uma letra maiúscula, minúscula e um número.Deve ser no mínimo 6 caracteres")]
        public string Senha { get; set; }
        [DataType(DataType.Password)]
        [Compare("Senha")]
        [Display(Name = "Confirma Senha")]
        public string ConfirmaSenha { get; set; }
    }
}