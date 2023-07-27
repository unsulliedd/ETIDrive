using ETIDrive_Data.Abstract;
using ETIDrive_Entity;

namespace ETIDrive_Data.Concrete
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(ETIDriveContext context) : base(context)
        {
        }
    }
}
