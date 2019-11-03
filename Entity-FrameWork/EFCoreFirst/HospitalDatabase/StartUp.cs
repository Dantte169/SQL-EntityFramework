using P01_HospitalDatabase.Data.Models;
using System;

namespace P01_HospitalDatabase
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            HospitalDbContext db = new HospitalDbContext();
            db.Database.EnsureCreated();
        }
    }
}
