using Shoes_shop.Data;
using System.Linq.Expressions;

namespace Shoes_shop.Models.Repositories
{
    public abstract class GenericRepository<T> : IBaseRepository<T> where T : class
    {
        public ApplicationDbContext Context { get; }
        public GenericRepository(ApplicationDbContext _Context)
        {
            Context = _Context;
        }

        public virtual T Add(T entity)
        {
            return Context.Add(entity).Entity;
        }

        public virtual IEnumerable<T> All()
        {
            return Context.Set<T>().ToList();
          //  return Context.Set<T>().Skip(1).Take(2).ToList();
        }

        public virtual T Delete(T entity)
        {
            return Context.Remove(entity).Entity;
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().AsQueryable().Where(predicate).ToList();
        }

        public virtual T FindOne(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().FirstOrDefault(predicate);

        }

        public virtual T Get(int id)
        {
            return Context.Find<T>(id);

        }

        public void CommitChanges()
        {
            Context.SaveChanges();  
        }

        public virtual T Update(T entity)
        {
            return Context.Update(entity).Entity;
        }
    }
}
