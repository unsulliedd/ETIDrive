using ETIDrive_Entity.Identity;
using ETIDrive_Entity;

namespace ETIDrive_Data.Abstract
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        void AddUserToDepartment(User user, Department department);
        void RemoveUserFromDepartment(User user, int departmentId);
        Task<List<User>> GetUserbyDepartment(int departmentId);
        Task<Folder>? GetDepartmentFolder(int departmentId);
        bool IsInDepartment(string userId, int departmentId);
    }
}