using ETIDrive_Data.Abstract;
using ETIDrive_Entity;

namespace ETIDrive_Data.Concrete
{
    public class DataRepository : GenericRepository<Data>, IDataRepository
    {
        private new readonly ETIDriveContext context;

        public DataRepository(ETIDriveContext context) : base(context)
        {
            this.context = context;
        }
        public List<string> GetFileTypes()
        {
            var fileTypes = context.Datas.Select(file => file.Category.Name).Distinct().ToList();
            return fileTypes;
        }

        public List<string> GetTags()
        {
            var tags = context.Tags.Select(tag => tag.Name).Distinct().ToList();
            return tags;
        }
    }
}
