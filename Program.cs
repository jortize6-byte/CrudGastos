using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Gimnasio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SocioRepositorio repo = new SocioRepositorio();
            int opcion;

            do
            {
                Console.WriteLine();
                Console.WriteLine("===== CRUD DE SOCIOS =====");
                Console.WriteLine("1. Registrar socio");
                Console.WriteLine("2. Listar socios");
                Console.WriteLine("3. Buscar socio por Id");
                Console.WriteLine("4. Actualizar cuota");
                Console.WriteLine("5. Eliminar socio");
                Console.WriteLine("6. Salir");
                Console.Write("Elija una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                    opcion = -1;

                switch (opcion)
                {
                    case 1: Registrar(repo); break;
                    case 2: Listar(repo); break;
                    case 3: Buscar(repo); break;
                    case 4: Actualizar(repo); break;
                    case 5: Eliminar(repo); break;
                    case 6: Console.WriteLine("Saliendo del programa..."); break;
                    default: Console.WriteLine("Opción no válida."); break;
                }

            } while (opcion != 6);
        }

        private static void Registrar(SocioRepositorio repo)
        {
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Membresía: ");
            string membresia = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Cuota: ");
            string cuotaTexto = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(membresia) ||
                !decimal.TryParse(cuotaTexto, out decimal cuota) ||
                cuota <= 0)
            {
                Console.WriteLine("Datos inválidos.");
                return;
            }

            try
            {
                Socio nuevo = new Socio
                {
                    Nombre = nombre,
                    Cuota = cuota,
                    Membresia = membresia
                };

                repo.Insertar(nuevo);
                Console.WriteLine("Socio registrado.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al registrar: {ex.Message}");
            }
        }

        private static void Listar(SocioRepositorio repo)
        {
            try
            {
                List<Socio> lista = repo.ListarTodos();

                if (lista.Count == 0)
                {
                    Console.WriteLine("No hay socios registrados.");
                    return;
                }

                decimal total = 0;
                foreach (Socio s in lista)
                {
                    Console.WriteLine(s);
                    total += s.Cuota;
                }

                Console.WriteLine($"TOTAL CUOTAS: Q {total:N2}");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al listar: {ex.Message}");
            }
        }

        private static void Buscar(SocioRepositorio repo)
        {
            Console.Write("Id del socio: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            try
            {
                Socio? socio = repo.BuscarPorId(id);

                if (socio == null)
                    Console.WriteLine("No existe ese Id.");
                else
                    Console.WriteLine(socio);
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al buscar: {ex.Message}");
            }
        }

        private static void Actualizar(SocioRepositorio repo)
        {
            Console.Write("Id del socio: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            try
            {
                Socio? socio = repo.BuscarPorId(id);

                if (socio == null)
                {
                    Console.WriteLine("No existe ese Id.");
                    return;
                }

                Console.WriteLine($"Socio actual: {socio}");
                Console.Write("Nueva cuota: ");

                if (!decimal.TryParse(Console.ReadLine(), out decimal nuevaCuota) || nuevaCuota <= 0)
                {
                    Console.WriteLine("Cuota inválida.");
                    return;
                }

                socio.Cuota = nuevaCuota;

                if (repo.Actualizar(socio))
                    Console.WriteLine("Cuota actualizada.");
                else
                    Console.WriteLine("No se pudo actualizar.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al actualizar: {ex.Message}");
            }
        }

        private static void Eliminar(SocioRepositorio repo)
        {
            Console.Write("Id del socio: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            try
            {
                if (repo.Eliminar(id))
                    Console.WriteLine("Socio eliminado.");
                else
                    Console.WriteLine("No existe ese Id.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al eliminar: {ex.Message}");
            }
        }
    }
}