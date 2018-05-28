using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CETYS.Posgrado.imi359.Clasificador.Models;

namespace CETYS.Posgrado.imi359.Clasificador
{
    public class Controlador
    {
         public void Iniciar()
        {
            var perturbaciones = ObtenerNuevasPerturbaciones();

            using (var context = new BdContext())
            {
                foreach (var i in perturbaciones)
                {
                    // Obtener los datos de la perturbacion
                    var perturbacion = context.Perturbacion
                        .Include(raw => raw.PerturbacionValor)
                        .Include(raw => raw.Humano)
                        .Single(t => t.PerturbacionId == i.PerturbacionId);

                    // Correr el algoritmo para determinar si es humano
                    Console.WriteLine($"Calculado para la perturbacion {i.PerturbacionId} ...");
                    Algoritmos.IAlgoritmo algoritmo = new Algoritmos.Basico();
                    if (!algoritmo.Correr(perturbacion))
                    {
                        Console.WriteLine("Perturbacion NO es humano");
                        continue;
                    }

                    Console.WriteLine("Perturbacion es humano");
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        Console.WriteLine($"Prendiendo la bandera de calculado para perturbacion {i.PerturbacionId} ...");
                        // Prender la bandera de que ya se calculo la perturbacion
                        perturbacion.Calculado = true;
                        context.SaveChanges();

                        Console.WriteLine($"Grabando registro de identificacion humana ...");
                        // Agregar registro a Human
                        var newHumano = new Humano
                        {
                            PerturbacionId = i.PerturbacionId,
                            FechaIdentificacion = DateTime.Now
                        };
                        context.Humano.Add(newHumano);
                        context.SaveChanges();

                        transaction.Commit();
                        Console.WriteLine("Transaccion completada");
                    }
                }
            }

        }

        private PerturbacionSoloLectura[] ObtenerNuevasPerturbaciones()
        {
            PerturbacionSoloLectura[] r;

            using (var context = new BdContext())
            {
                r = context.PerturbacionLista.FromSql("CALL obtenerPerturbacionesSinCalcular()").ToArray();
                if (r.Length != 0) Console.WriteLine($"Se encontratron {r.Length} nuevas perturbaciones");
            }

            return r;
        }
    }
}
