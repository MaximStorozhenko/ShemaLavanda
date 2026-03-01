using Microsoft.EntityFrameworkCore;
using ShemaLavanda.Data;
using ShemaLavanda.Models;

namespace ShemaLavanda.Services
{
    public class EquipmentService
    {
        public List<Equipment> GetAll()
        {
            using AppDbContext db = new();
            return db.Equipments
                     .AsNoTracking()
                     .ToList();
        }

        public Equipment GetByCode(string code)
        {
            using AppDbContext db = new();
            return db.Equipments
                     .AsNoTracking()
                     .FirstOrDefault(e => e.Code == code);
        }

        public void Add(Equipment equipment)
        {
            using AppDbContext db = new();
            db.Equipments.Add(equipment);
            db.SaveChanges();
        }

        public void Update(Equipment equipment)
        {
            using AppDbContext db = new();
            db.Equipments.Update(equipment);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            using AppDbContext db = new();
            Equipment eq = db.Equipments.Find(id);
            if (eq != null)
            {
                db.Equipments.Remove(eq);
                db.SaveChanges();
            }
        }
    }
}
