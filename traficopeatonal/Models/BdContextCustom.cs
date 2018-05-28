using Microsoft.EntityFrameworkCore;

namespace CETYS.Posgrado.imi359.Servicios.Models
{
    public partial class BdContext : DbContext
    {
        public virtual DbSet<HumanoSoloLectura> HumanoLista { get; set; }

        public BdContext(DbContextOptions<Servicios.Models.BdContext> options)
            : base(options)
        { }

    }
}
