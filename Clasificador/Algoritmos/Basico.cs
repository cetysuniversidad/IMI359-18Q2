using System;
using CETYS.Posgrado.imi359.Clasificador.Models;

namespace CETYS.Posgrado.imi359.Clasificador.Algoritmos
{
    public class Basico : IAlgoritmo
    {
        public bool Correr(Perturbacion perturbacion)
        {
            // hacer el random de false y true
            var random = new Random();
            // Con 70% de probabilidad de que salga True
            return random.NextDouble() < 0.70;
        }
    }
}
