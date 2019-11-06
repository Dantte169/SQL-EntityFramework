using InitialSetup;
using System;
using System.Data.SqlClient;

namespace RemoveVillian
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            using SqlConnection connection = new SqlConnection(Setup.connectionData);
            connection.Open();

            using SqlCommand command = new SqlCommand(Queries.TakeVillainName, connection);
            command.Parameters.AddWithValue("@villainId", id);

            string villainName = (string)command.ExecuteScalar();

            if (villainName == null)
            {
                Console.WriteLine("No such villain was found.");
                return;
            }

            using SqlCommand deleteVillainMinions = new SqlCommand(Queries.DeleteVillainMinions, connection);
            deleteVillainMinions.Parameters.AddWithValue("@villainId", id);

            int count = deleteVillainMinions.ExecuteNonQuery();

            using (SqlCommand deleteVillain = new SqlCommand(Queries.DeleteVillain, connection))
            {
                deleteVillain.Parameters.AddWithValue("@villainId", id);
                deleteVillain.ExecuteNonQuery();
            }

            Console.WriteLine($"{villainName} was deleted.");
            Console.WriteLine($"{count} minions were released.");
        }
    }
}
