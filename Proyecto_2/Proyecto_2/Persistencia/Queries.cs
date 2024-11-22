using Proyecto_2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace Proyecto_2.Persistencia
{
    public class Queries
    {
        private string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CalculadoraDBContext"].ConnectionString;
        private readonly CalculadoraCientifica calculadoraCientifica;
        public Queries()
        {
            calculadoraCientifica = new CalculadoraCientifica();

        }
        public List<CalculadoraCientifica> ObtenerTodos()
        {
            List<CalculadoraCientifica> resultados = new List<CalculadoraCientifica>();

            string query = "SELECT Numero1, Operacion, Numero2, Resultado FROM calculadoraCientifica";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CalculadoraCientifica calculadora = new CalculadoraCientifica
                    {
                        Numero1 = reader.GetDecimal(0),
                        Operacion = reader.GetString(1),
                        Numero2 = reader.GetDecimal(2),
                        Resultado = reader.GetDecimal(3)
                    };

                    resultados.Add(calculadora);
                }
            }

            return resultados;
        }

    

 
        public void AddOperacion(decimal numero1, string operacion, decimal numero2, decimal resultado)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO Operaciones (Numero1, Operacion, Numero2, Resultado, FechaCalculo) " +
                                   "VALUES (@Numero1, @Operacion, @Numero2, @Resultado, @FechaCalculo)";

            
                    SqlCommand command = new SqlCommand(query, connection);

   
                    command.Parameters.Add("@Numero1", SqlDbType.Decimal).Value = numero1;
                    command.Parameters.Add("@Operacion", SqlDbType.NVarChar, 10).Value = operacion;
                    command.Parameters.Add("@Numero2", SqlDbType.Decimal).Value = numero2;
                    command.Parameters.Add("@Resultado", SqlDbType.Decimal).Value = resultado;
                    command.Parameters.Add("@FechaCalculo", SqlDbType.DateTime).Value = DateTime.Now; 

     
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        public List<CalculadoraCientifica> GetOperationsByOperacion(string Operacion)
        {

            string operacionModificada = Operacion?.Replace("+", "+");

            List<CalculadoraCientifica> operaciones = new List<CalculadoraCientifica>();


            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM dbo.calculadoraCientifica WHERE Operacion LIKE @Operacion";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
               
                        command.Parameters.Add("@Operacion", SqlDbType.NVarChar, 10).Value = Operacion;


                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CalculadoraCientifica operacionObj = new CalculadoraCientifica
                                {
                                    Numero1 = reader.GetDecimal(reader.GetOrdinal("Numero1")),
                                    Operacion = reader.GetString(reader.GetOrdinal("Operacion")),
                                    Numero2 = reader.GetDecimal(reader.GetOrdinal("Numero2")),
                                    Resultado = reader.GetDecimal(reader.GetOrdinal("Resultado"))
                                };
                                operaciones.Add(operacionObj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            return operaciones;
        }

    }
}