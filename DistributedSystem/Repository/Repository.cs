using DistributedSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace DistributedSystem.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        private readonly DbContext DbContext;

        protected Repository(DbContext dbContext) => DbContext = dbContext;

        public DbSet<T> Set => DbContext.Set<T>();

        public async Task<T?> Add(T entity)
        {
            await DbContext.AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return await GetById(entity.Id);
        }

        public async Task<T?> Update(T entity)
        {
            DbContext.Update(entity);
            await DbContext.SaveChangesAsync();
            return await GetById(entity.Id);
        }


        public async Task<T?> GetById(long id)
        {
            return await Set.SingleOrDefaultAsync(entity => entity.Id == id);
        }

        public IQueryable<T> GetAll()
        {
            return Set.AsNoTracking();
        }
    }
}
