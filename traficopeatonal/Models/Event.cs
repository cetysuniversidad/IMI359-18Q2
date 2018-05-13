using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CETYS.Posgrado.imi359.Models
{
    [Table("event")]
    public partial class Event
    {
        public Event()
        {
            EventValueComputed = new HashSet<EventValueComputed>();
            EventValueRaw = new HashSet<EventValueRaw>();
            Human = new HashSet<Human>();
        }

        [Column("event_id", TypeName = "bigint(20)")]
        public long EventId { get; set; }
        [Column("time_elapsed", TypeName = "int(11)")]
        public int TimeElapsed { get; set; }
        [Column("is_computed", TypeName = "bit(1)")]
        public bool IsComputed { get; set; }
        [Column("create_date", TypeName = "datetime")]
        public DateTime CreateDate { get; set; }

        [InverseProperty("Event")]
        public ICollection<EventValueComputed> EventValueComputed { get; set; }
        [InverseProperty("Event")]
        public ICollection<EventValueRaw> EventValueRaw { get; set; }
        [InverseProperty("Event")]
        public ICollection<Human> Human { get; set; }
    }
}
