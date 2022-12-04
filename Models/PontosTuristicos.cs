using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LocalWise.Models
{
    public class PontosTuristicos
    {
        [Key]
        public int Id { get; set; }

        public string Foto { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Descrição")]
        [DataType(DataType.MultilineText)]
        [MaxLength(200)]
        public string Descricao { get; set; }

        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        public virtual ICollection<PontosGuias> PontosGuias { get; set; }
    }
}