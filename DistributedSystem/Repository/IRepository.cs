using DistributedSystem.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DistributedSystem.Repository
{
    public interface IRepository<T> where T : BaseModel
    {
        public Task<T?> Add(T entity);
        public Task<T?> Update(T entity);

        public Task<T?> GetById(long id);

        public IQueryable<T> GetAll();
    }
}
