using CETYS.Posgrado.imi359.Clasificador.Models;

namespace CETYS.Posgrado.imi359.Clasificador.Algoritmos
{
    public interface IAlgoritmo
    {
        bool Correr(Perturbacion perturbacion);
    }
}
