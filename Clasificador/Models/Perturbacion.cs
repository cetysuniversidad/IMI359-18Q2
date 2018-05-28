using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CETYS.Posgrado.imi359.Clasificador.Models
{
    [Table("perturbacion")]
    public partial class Perturbacion
    {
        public Perturbacion()
        {
            Humano = new HashSet<Humano>();
            PerturbacionValor = new HashSet<PerturbacionValor>();
        }

        [Column("perturbacion_id", TypeName = "bigint(20)")]
        public long PerturbacionId { get; set; }
        [Column("duracion", TypeName = "int(11)")]
        public int Duracion { get; set; }
        [Column("calculado", TypeName = "bit(1)")]
        public bool Calculado { get; set; }
        [Column("etiqueta", TypeName = "datetime")]
        public DateTime Etiqueta { get; set; }

        [InverseProperty("Perturbacion")]
        public ICollection<Humano> Humano { get; set; }
        [InverseProperty("Perturbacion")]
        public ICollection<PerturbacionValor> PerturbacionValor { get; set; }
    }
}
