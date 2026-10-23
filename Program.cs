using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace CrudGastos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GastoRepositorio repo = new GastoRepositorio();
            int opcion;

            do
            {
                Console.WriteLine();
                Console.WriteLine("===== CRUD DE GASTOS =====");
                Console.WriteLine("1. Registrar gasto");
                Console.WriteLine("2. Listar gastos");
                Console.WriteLine("3. Buscar gasto por Id");
                Console.WriteLine("4. Actualizar monto");
                Console.WriteLine("5. Eliminar gasto");
                Console.WriteLine("6. Salir");
                Console.Write("Elija una de las 6  opción: ");

                string entrada = Console.ReadLine() ?? "";
                if (!int.TryParse(entrada, out opcion))
                {
                    opcion = -1;
                }

                switch (opcion)
                {
                    case 1:
                        RegistrarGasto(repo);
                        break;
                    case 2:
                        ListarGastos(repo);
                        break;
                    case 3:
                        BuscarGasto(repo);
                        break;
                    case 4:
                        ActualizarMonto(repo);
                        break;
                    case 5:
                        EliminarGasto(repo);
                        break;
                    case 6:
                        Console.WriteLine("Saliendo del programa...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida no es una de las 6 opciones.");
                        break;
                }

            } while (opcion != 6);
        }

        private static void RegistrarGasto(GastoRepositorio repo)
        {
            Console.Write("Descripción: ");
            string descripcion = Console.ReadLine() ?? "";

            Console.Write("Monto: ");
            string montoTexto = Console.ReadLine() ?? "";

            Console.Write("Categoría: ");
            string categoria = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(descripcion) ||
                string.IsNullOrWhiteSpace(categoria) ||
                !decimal.TryParse(montoTexto, out decimal monto) ||
                monto <= 0)
            {
                Console.WriteLine("Datos inválidos.");
                return;
            }

            try
            {
                Gasto nuevo = new Gasto
                {
                    Descripcion = descripcion.Trim(),
                    Monto = monto,
                    Categoria = categoria.Trim()
                };

                repo.Insertar(nuevo);
                Console.WriteLine("Gasto registrado.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al registrar: {ex.Message}");
            }
        }

        private static void ListarGastos(GastoRepositorio repo)
        {
            try
            {
                List<Gasto> gastos = repo.ListarTodos();

                if (gastos.Count == 0)
                {
                    Console.WriteLine("No hay gastos registrados.");
                    return;
                }

                decimal total = 0;
                foreach (Gasto g in gastos)
                {
                    Console.WriteLine(g.ToString());
                    total += g.Monto;
                }

                Console.WriteLine($"TOTAL GASTADO: Q {total:N2}");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al listar: {ex.Message}");
            }
        }

        private static void BuscarGasto(GastoRepositorio repo)
        {
            Console.Write("Id del gasto: ");
            string idTexto = Console.ReadLine() ?? "";

            if (!int.TryParse(idTexto, out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            try
            {
                Gasto? gasto = repo.BuscarPorId(id);

                if (gasto == null)
                    Console.WriteLine("No existe ese Id.");
                else
                    Console.WriteLine(gasto.ToString());
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al buscar: {ex.Message}");
            }
        }

        private static void ActualizarMonto(GastoRepositorio repo)
        {
            Console.Write("Id del gasto: ");
            string idTexto = Console.ReadLine() ?? "";

            if (!int.TryParse(idTexto, out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            try
            {
                Gasto? gasto = repo.BuscarPorId(id);

                if (gasto == null)
                {
                    Console.WriteLine("No existe ese Id.");
                    return;
                }

                Console.WriteLine($"Gasto actual: {gasto}");
                Console.Write("Nuevo monto: ");
                string montoTexto = Console.ReadLine() ?? "";

                if (!decimal.TryParse(montoTexto, out decimal nuevoMonto) || nuevoMonto <= 0)
                {
                    Console.WriteLine("Monto inválido.");
                    return;
                }

                gasto.Monto = nuevoMonto;
                bool ok = repo.Actualizar(gasto);

                if (ok)
                    Console.WriteLine("Gasto actualizado correctamente.");
                else
                    Console.WriteLine("No existe ese Id.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al actualizar: {ex.Message}");
            }
        }

        private static void EliminarGasto(GastoRepositorio repo)
        {
            Console.Write("Id del gasto: ");
            string idTexto = Console.ReadLine() ?? "";

            if (!int.TryParse(idTexto, out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            try
            {
                bool ok = repo.Eliminar(id);

                if (ok)
                    Console.WriteLine("Gasto eliminado correctamente.");
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