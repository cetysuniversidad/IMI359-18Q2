using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CETYS.Posgrado.imi359.Models
{
    [Table("human")]
    public partial class Human
    {
        [Column("human_id", TypeName = "bigint(20)")]
        public long HumanId { get; set; }
        [Column("event_id", TypeName = "bigint(20)")]
        public long EventId { get; set; }
        [Column("identification_date", TypeName = "datetime")]
        public DateTime IdentificationDate { get; set; }

        [ForeignKey("EventId")]
        [InverseProperty("Human")]
        public Event Event { get; set; }
    }
}
