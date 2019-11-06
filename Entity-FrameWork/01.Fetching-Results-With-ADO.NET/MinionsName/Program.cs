using InitialSetup;
using System;
using System.Data.SqlClient;

namespace MinionsName
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());
            using SqlConnection connection = new SqlConnection(Setup.connectionData);
            connection.Open();

            using (SqlCommand command = new SqlCommand(Queries.VillainName, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                string villianName = (string)command.ExecuteScalar();

                if (villianName == null)
                {
                    Console.WriteLine($"No villain with ID {id} exists in the database.");
                    return;
                }
                Console.WriteLine($"Villain: {villianName}");
            }

            using (SqlCommand command = new SqlCommand(Queries.MinionNames, connection))
            {

                command.Parameters.AddWithValue("@Id", id);

                using SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    Console.WriteLine("(no minions)");
                    return;
                }

                while (reader.Read())
                {
                    long row = (long)reader[0];
                    string name = (string)reader[1];
                    int age = (int)reader[2];

                    Console.WriteLine($"{row}. {name} {age}");
                }

            }
        }
    }
}
