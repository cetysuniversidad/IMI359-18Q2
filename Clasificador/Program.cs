using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace CETYS.Posgrado.imi359.Clasificador
{
    class Program
    {
        static void Main(string[] args)
        {
            new Controlador().Iniciar();

            Console.WriteLine("Proceso completado");
            Console.ReadKey();
        }
    }
}
