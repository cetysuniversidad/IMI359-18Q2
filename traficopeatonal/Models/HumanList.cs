using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CETYS.Posgrado.imi359.Models
{
    public class HumanList
    {
        [Column("RowNumber", TypeName = "int(11)")]
        [Key]
        [JsonIgnore]
        public int RowNumber { get; set; }

        [Column("Date", TypeName = "varchar(10)")]
        public string Date { get; set; }

        [Column("Time", TypeName = "varchar(10)")]
        public string Time { get; set; }

        [Column("Count", TypeName = "int(11)")]
        public int Count { get; set; }
    }
}
