using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VäderLabb;

namespace ProjektVäderdataDataAccess
{
    public class DatabaseContext : DbContext
    {
        public DbSet<WeatherRecord> WeatherRecords { get; set; }

        //Connection string och döper databasen till WeatherDB
        private const string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=WeatherDB;Trusted_Connection=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString); 
            //Använder SQLServer LocalDB
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherRecord>(entity =>
            {
                entity.HasKey(e => e.Id); //Sätter Id som primary key
                entity.HasIndex(e => e.Datum); //Index på Datum för snabbare sökningar
                entity.HasIndex(e => e.Plats); //Index på Plats för snabbare sökningar
                entity.Ignore(e => e.MoldRisk); //Ignorerar MoldRisk eftersom det är en beräknad egenskap
            });
        }
    }
}
