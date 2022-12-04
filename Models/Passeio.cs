using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LocalWise.Models
{
    public class Passeio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Cidade { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public string Data { get; set; }

        [Required]
        public int QtdPessoas { get; set; }
    }
}