using ETIDrive_Entity;

namespace ETIDrive_Data.Abstract
{
    public interface IDataRepository : IGenericRepository<Data>
    {
        List<string> GetFileTypes();
        List<string> GetTags();
    }
}
