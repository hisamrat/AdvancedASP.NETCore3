using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AdvancedASP.NETCore3.DataAccess.Data.Repository.IRepository
{
   public interface IRepository<T> where T:class
    {
        //ICollection<T> FindAll();
        //T FindById(int id);
        //bool isExists(int id);
        //bool Create(T entity);
        //bool Update(T entity);
        //bool Delete(T entity);
        //bool Save();


        T Get(int id);
        IEnumerable<T> GetAll(
            Expression<Func<T,bool>> filter=null,
            Func<IQueryable<T>,IOrderedQueryable<T>> orderBy=null,
            string includeProperties=null
            );

        T GetFirstOrDefault(
           Expression<Func<T, bool>> filter = null,
            string includeProperties = null

            );

        void Add(T entity);
        void Remove(int id);
        void Remove(T entity);
    }
}
