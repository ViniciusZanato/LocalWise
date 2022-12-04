using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LocalWise.Models
{
    public class PontosGuias
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Guia")]
        public int GuiaId { get; set; }

        [Display(Name = "Pontos Turísticos")]
        public int PontosTuristicosId { get; set; }

        public virtual Guia Gui { get; set; }

        public virtual PontosTuristicos PontosTuristicos { get; set; }
    }
}