using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LocalWise.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        public int Status { get; set; }

        [Required]
        public string Cidade { get; set; }

        [Required]
        public int QtdPessoas { get; set; }

        [Required]
        public string Locomocao { get; set; }

        [Required]
        public string Tipo { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public string Data { get; set; }

        public int GuiaId { get; set; }

        public virtual Guia Guia { get; set; }

        public int TuristaId { get; set; }

        public virtual Turista Turista { get; set; }
    }
}