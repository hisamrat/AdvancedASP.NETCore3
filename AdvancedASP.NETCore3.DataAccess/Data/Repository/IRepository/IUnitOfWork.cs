﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedASP.NETCore3.DataAccess.Data.Repository.IRepository
{
   public interface IUnitOfWork:IDisposable
    {
        ICategoryRepository Category { get; }
        IFrequencyRepository Frequency { get; }
        IServiceRepository Service { get; }
        void Save();
    }
}
