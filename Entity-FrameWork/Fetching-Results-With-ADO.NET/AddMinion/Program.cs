using InitialSetup;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace AddMinion
{
    public class Program
    {
        public static int TownId;
        public static int MinionId;
        public static int VillainId;
        public static void Main(string[] args)
        {
            List<string> minionsItems = Console.ReadLine()
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1)
                    .ToList();

            string minionName = minionsItems[0];
            int minionAge = int.Parse(minionsItems[1]);
            string minionTown = minionsItems[2];

            List<string> villainsItems = Console.ReadLine()
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1)
                    .ToList();

            string villainName = villainsItems[0];

            using (SqlConnection dbConnection = new SqlConnection(Setup.connectionData))
            {
                AddMinionTown(minionTown, dbConnection);
                AddMinion(minionName, minionAge, dbConnection);
                AddVillian(villainName, dbConnection);
                AddMinionToVillian(minionName, villainName, dbConnection);
            }
        }

        private static void AddMinionToVillian(string minionName, string villainName, SqlConnection dbConnection)
        {
            using SqlCommand AddMinion = new SqlCommand(Queries.InsertMV, dbConnection);
            AddMinion.Parameters.AddWithValue("@villianId", VillainId);
            AddMinion.Parameters.AddWithValue("@minionId", MinionId);
            AddMinion.ExecuteNonQuery();

            Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
        }

        private static void AddVillian(string villainName, SqlConnection dbConnection)
        {
            using SqlCommand checkVillian = new SqlCommand(Queries.TakeVillainId, dbConnection);
            checkVillian.Parameters.AddWithValue("@Name", villainName);

            object villianID = checkVillian.ExecuteScalar();

            if (villianID != null)
            {
                VillainId = (int)villianID;
            }
            else
            {
                using SqlCommand insertVillian = new SqlCommand(Queries.InsertVillain, dbConnection);
                insertVillian.Parameters.AddWithValue("@villianName", villainName);
                insertVillian.ExecuteNonQuery();

                Console.WriteLine($"Villain {villainName} was added to the database.");
            }
        }

        private static void AddMinion(string minionName, int minionAge, SqlConnection dbConnection)
        {
            using SqlCommand checkMinion = new SqlCommand(Queries.TakeMinionId, dbConnection);
            checkMinion.Parameters.AddWithValue("@Name", minionName);

            int minionID = (int)checkMinion.ExecuteScalar();

            if (minionID != null)
            {
                MinionId = (int)minionID;
            }
            else
            {
                using SqlCommand insertMinion = new SqlCommand(Queries.InsertMinion, dbConnection);
                insertMinion.Parameters.AddWithValue("@nam",minionName);
                insertMinion.Parameters.AddWithValue("@age",minionAge);
                insertMinion.Parameters.AddWithValue("@townId",TownId);

                Console.WriteLine($"Minion {minionName} was added to the database.");
            }

        }

        private static void AddMinionTown(string minionTown, SqlConnection dbConnection)
        {
            using SqlCommand checkTownCommand = new SqlCommand(Queries.TakeTownId, dbConnection);
            checkTownCommand.Parameters.AddWithValue("@townName", minionTown);

            object townID = checkTownCommand.ExecuteScalar();

            if (townID != null)
            {
                TownId = (int)townID;
            }
            else
            {
                using SqlCommand insertTown = new SqlCommand(Queries.InsertTown, dbConnection);
                insertTown.Parameters.AddWithValue("@townName", minionTown);
                insertTown.ExecuteNonQuery();

                Console.WriteLine($"Town {minionTown} was added to the database.");
            }

        }
    }
}
