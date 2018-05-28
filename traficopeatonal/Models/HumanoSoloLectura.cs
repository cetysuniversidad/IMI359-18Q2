using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CETYS.Posgrado.imi359.Servicios.Models
{
    public class HumanoSoloLectura
    {
        [Column("Renglon", TypeName = "int(11)")]
        [Key]
        [JsonIgnore]
        public int Renglon { get; set; }

        [Column("Fecha", TypeName = "varchar(10)")]
        public string Fecha { get; set; }

        [Column("Tiempo", TypeName = "varchar(10)")]
        public string Tiempo { get; set; }

        [Column("Total", TypeName = "int(11)")]
        public int Total { get; set; }
    }
}
