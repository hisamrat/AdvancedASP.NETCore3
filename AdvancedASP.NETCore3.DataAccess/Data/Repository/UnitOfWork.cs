using AdvancedASP.NETCore3.DataAccess.Data.Repository.IRepository;
using AdvancedASP.NETCore3.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedASP.NETCore3.DataAccess.Data.Repository
{
   public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Frequency = new FrequencyRepository(_db);
            Service = new ServiceRepository(_db);
        }
        public ICategoryRepository Category { get;private set; }

        public IFrequencyRepository Frequency { get; private set; }
        public IServiceRepository Service { get; set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
