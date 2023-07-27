using ETIDrive_Data.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ETIDrive_Data.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> 
        where T : class
    {
        protected readonly DbContext context;
        public GenericRepository(DbContext context)
        {
            this.context = context;
        }

        public T GetbyId(int id)
        {
            return context.Set<T>().Find(id);
        }
        public List<T> GetAll()
        {
            return context.Set<T>().ToList();
        }
        public void Create(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }
        public async Task<T> GetbyIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task CreateAsync(T entity)
        {
            context.Set<T>().Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
