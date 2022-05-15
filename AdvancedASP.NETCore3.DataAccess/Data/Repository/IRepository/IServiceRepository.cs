using AdvancedASP.NETCore3.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedASP.NETCore3.DataAccess.Data.Repository.IRepository
{
   public interface IServiceRepository:IRepository<Service>
    {
        void Update(Service service);
    }
}
