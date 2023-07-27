using ETIDrive_Data.Abstract;
using ETIDrive_Entity;

namespace ETIDrive_Data.Concrete
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ETIDriveContext context) : base(context)
        {
        }
    }
}
