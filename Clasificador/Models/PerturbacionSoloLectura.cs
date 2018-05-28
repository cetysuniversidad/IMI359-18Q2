using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Newtonsoft.Json;

namespace CETYS.Posgrado.imi359.Clasificador.Models
{
    public class PerturbacionSoloLectura
    {
        [Column("perturbacion_id", TypeName = "bigint(20)")]
        [Key]
        public long PerturbacionId { get; set; }
    }
}
