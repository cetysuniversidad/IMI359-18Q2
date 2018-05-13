using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CETYS.Posgrado.imi359.Models
{
    [Table("event_value_raw")]
    public partial class EventValueRaw
    {
        [Column("event_value_raw_id", TypeName = "bigint(20)")]
        public long EventValueRawId { get; set; }
        [Column("event_id", TypeName = "bigint(20)")]
        public long EventId { get; set; }
        [Column("value")]
        public float Value { get; set; }

        [ForeignKey("EventId")]
        [InverseProperty("EventValueRaw")]
        public Event Event { get; set; }
    }
}
