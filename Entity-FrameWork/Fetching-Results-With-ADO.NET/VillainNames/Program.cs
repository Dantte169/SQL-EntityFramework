using InitialSetup;
using System;
using System.Data.SqlClient;

namespace VillainNames
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using SqlConnection connection = new SqlConnection(Setup.connectionData);
            connection.Open();

            using SqlCommand command = new SqlCommand(Queries.TopMinions, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string name = (string)reader[0];
                int count = (int)reader[1];

                Console.WriteLine($"{name} - {count}");
            }
        }
    }
}
