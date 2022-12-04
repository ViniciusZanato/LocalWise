using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LocalWise.Models
{
    public class TipoPasseio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Tipo { get; set; }
    }
}