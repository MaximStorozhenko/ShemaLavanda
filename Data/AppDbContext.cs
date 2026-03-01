using Microsoft.EntityFrameworkCore;
using ShemaLavanda.Models;
using System.IO;

namespace ShemaLavanda.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Equipment> Equipments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.db");

            optionsBuilder.UseSqlite($"Data Source={path}");
        }
    }
}
