using InitialSetup;
using System;
using System.Data;
using System.Data.SqlClient;

namespace IncreaseAgeProcedure
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            using SqlConnection connection = new SqlConnection(Setup.connectionData);
            connection.Open();

            using (SqlCommand command = new SqlCommand(Queries.CreateProcedure, connection))
            {
                command.ExecuteNonQuery();
            }

            using (SqlCommand command = new SqlCommand("usp_GetOlder", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }

            using (SqlCommand command = new SqlCommand(Queries.SelectMinion, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string name = (string)reader[0];
                    int age = (int)reader[1];

                    Console.WriteLine($"{name} – {age} years old");
                }
            }
        }
    }
}
