namespace ETIDrive_Data.Abstract
{
    public interface IGenericRepository<T>
    {
        T GetbyId(int id);
        List<T> GetAll();
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);

        //Async
        Task<T> GetbyIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
