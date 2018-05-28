using System.ComponentModel.DataAnnotations.Schema;

namespace CETYS.Posgrado.imi359.Clasificador.Models
{
    [Table("perturbacion_valor")]
    public partial class PerturbacionValor
    {
        [Column("perturbacion_valor_id", TypeName = "bigint(20)")]
        public long PerturbacionValorId { get; set; }
        [Column("perturbacion_id", TypeName = "bigint(20)")]
        public long PerturbacionId { get; set; }
        [Column("valor")]
        public float Valor { get; set; }

        [ForeignKey("PerturbacionId")]
        [InverseProperty("PerturbacionValor")]
        public Perturbacion Perturbacion { get; set; }
    }
}
