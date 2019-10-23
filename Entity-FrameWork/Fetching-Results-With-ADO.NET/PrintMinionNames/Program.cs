using InitialSetup;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace PrintMinionNames
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<string> names = new List<string>();

            using (SqlConnection connection = new SqlConnection(Setup.connectionData))
            {
                connection.Open();

                using SqlCommand command = new SqlCommand(Queries.TakeAllMinionsNames, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    names.Add((string)reader[0]);
                }
            }

            Console.WriteLine($"Original Vision:\n{string.Join(Environment.NewLine, names)}");
            Console.WriteLine("New Vision:");

            while (names.Count != 0)
            {
                Console.WriteLine(names[0]);
                names.RemoveAt(0);

                if (names.Count == 0)
                {
                    break;
                }

                Console.WriteLine(names.Last());
                names.RemoveAt(names.Count - 1);
            }
        }
    }
}
