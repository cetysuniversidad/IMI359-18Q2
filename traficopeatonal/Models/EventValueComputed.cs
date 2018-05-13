using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CETYS.Posgrado.imi359.Models
{
    [Table("event_value_computed")]
    public partial class EventValueComputed
    {
        [Column("event_value_computed_id", TypeName = "bigint(20)")]
        public long EventValueComputedId { get; set; }
        [Column("event_id", TypeName = "bigint(20)")]
        public long EventId { get; set; }
        [Column("value")]
        public float Value { get; set; }

        [ForeignKey("EventId")]
        [InverseProperty("EventValueComputed")]
        public Event Event { get; set; }
    }
}
