using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CETYS.Posgrado.imi359.Servicios.Models
{
    [Table("humano")]
    public partial class Humano
    {
        [Column("humano_id", TypeName = "bigint(20)")]
        public long HumanoId { get; set; }
        [Column("perturbacion_id", TypeName = "bigint(20)")]
        public long PerturbacionId { get; set; }
        [Column("fecha_identificacion", TypeName = "datetime")]
        public DateTime FechaIdentificacion { get; set; }

        [ForeignKey("PerturbacionId")]
        [InverseProperty("Humano")]
        public Perturbacion Perturbacion { get; set; }
    }
}
