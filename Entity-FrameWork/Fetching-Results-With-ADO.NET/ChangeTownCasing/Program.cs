using InitialSetup;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ChangeTownCasing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string country = Console.ReadLine();

            using SqlConnection connection = new SqlConnection(Setup.connectionData);
            connection.Open();

            using (SqlCommand command = new SqlCommand(Queries.EditTownNames, connection))
            {
                command.Parameters.AddWithValue("@countryName", country);
                int count = command.ExecuteNonQuery();

                Console.WriteLine($"{count} town names were affected. ");
            }

            using (SqlCommand command = new SqlCommand(Queries.FindEditedTowns, connection))
            {
                command.Parameters.AddWithValue("@countryName", country);

                List<string> cities = new List<string>();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cities.Add((string)reader[0]);
                    }
                }

                if (cities.Count == 0)
                {
                    Console.WriteLine("No town names were affected.");
                }
                else
                {
                    Console.WriteLine($"[{string.Join(", ", cities)}]");
                }
            }
        }
    }
}
