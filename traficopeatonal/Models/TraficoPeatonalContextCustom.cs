using Microsoft.EntityFrameworkCore;

namespace CETYS.Posgrado.imi359.Models
{
    public partial class TraficoPeatonalContext : DbContext
    {
        public virtual DbSet<HumanList> HumanList { get; set; }

        public TraficoPeatonalContext(DbContextOptions<TraficoPeatonalContext> options)
            : base(options)
        { }

    }
}
