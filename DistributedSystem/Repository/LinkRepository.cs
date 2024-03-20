using DistributedSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace DistributedSystem.Repository
{
    public class LinkRepository : Repository<Link>
    {
        public LinkRepository(LinkDBContext dbContext) : base(dbContext)
        { }

        public async ValueTask<Link?> GetLink(long id) => await GetById(id);

        public async ValueTask<IReadOnlyCollection<Link>> GetLinks()
        {
            return await GetAll()
                .ToListAsync();
        }

        public async ValueTask<Link?> AddLink(Link Links) => await Add(Links);
        public async ValueTask<Link?> UpdateLink(Link Links) => await Update(Links);
    }
}
