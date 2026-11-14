using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Gimnasio
{
    public class SocioRepositorio
    {
        private const string CadenaConexion =
            @"Server=(localdb)\MSSQLLocalDB;" +
            @"Database=GimnasioDB;" +
            @"Trusted_Connection=True;" +
            @"TrustServerCertificate=True;";

        public void Insertar(Socio s)
        {
            string sql = @"INSERT INTO Socios (Nombre, Cuota, Membresia)
                           VALUES (@nombre, @cuota, @membresia)";

            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@nombre", s.Nombre);
                comando.Parameters.AddWithValue("@cuota", s.Cuota);
                comando.Parameters.AddWithValue("@membresia", s.Membresia);

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        public List<Socio> ListarTodos()
        {
            List<Socio> lista = new List<Socio>();
            string sql = @"SELECT Id, Nombre, Cuota, Membresia
                           FROM Socios
                           ORDER BY Id";

            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new Socio
                        {
                            Id = lector.GetInt32(0),
                            Nombre = lector.GetString(1),
                            Cuota = lector.GetDecimal(2),
                            Membresia = lector.GetString(3)
                        });
                    }
                }
            }
            return lista;
        }

        public Socio? BuscarPorId(int id)
        {
            string sql = @"SELECT Id, Nombre, Cuota, Membresia
                           FROM Socios
                           WHERE Id = @id";

            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@id", id);
                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        return new Socio
                        {
                            Id = lector.GetInt32(0),
                            Nombre = lector.GetString(1),
                            Cuota = lector.GetDecimal(2),
                            Membresia = lector.GetString(3)
                        };
                    }
                }
            }
            return null;
        }

        public bool Actualizar(Socio s)
        {
            string sql = @"UPDATE Socios
                           SET Cuota = @cuota
                           WHERE Id = @id";

            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@cuota", s.Cuota);
                comando.Parameters.AddWithValue("@id", s.Id);

                conexion.Open();
                return comando.ExecuteNonQuery() == 1;
            }
        }

        public bool Eliminar(int id)
        {
            string sql = @"DELETE FROM Socios WHERE Id = @id";

            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@id", id);
                conexion.Open();
                return comando.ExecuteNonQuery() == 1;
            }
        }
    }
}