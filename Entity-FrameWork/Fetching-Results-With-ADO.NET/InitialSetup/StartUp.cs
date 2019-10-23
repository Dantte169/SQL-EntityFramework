using System;
using System.Data.SqlClient;

namespace InitialSetup
{
    public class StartUp
    {

        public static void Main(string[] args)
        {
            using SqlConnection dbConnection = new SqlConnection(Setup.connectionData);

            dbConnection.Open();

            using (dbConnection)
            {
                string createDB = "CREATE DATABASE MinionsDB";

                try
                {
                    SqlCommand command = new SqlCommand(createDB, dbConnection);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Database Created.");

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error While Creating Database");
                    Console.WriteLine(e.Message);
                }

                try
                {
                    foreach (var procedure in Setup.CreateTableStatements)
                    {
                        using (SqlCommand command = new SqlCommand(procedure, dbConnection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    Console.WriteLine("Tables Created Succesfully.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                try
                {
                    foreach (var procedure in Setup.InsertDataStatements)
                    {
                        using (SqlCommand command = new SqlCommand(procedure, dbConnection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    Console.WriteLine("Data Inserted Succefully.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }

        }
    }
}
