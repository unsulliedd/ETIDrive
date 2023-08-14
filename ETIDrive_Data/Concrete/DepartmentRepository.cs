using ETIDrive_Data.Abstract;
using ETIDrive_Entity;
using ETIDrive_Entity.Identity;
using Microsoft.EntityFrameworkCore;

namespace ETIDrive_Data.Concrete
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private new readonly ETIDriveContext context;

        public DepartmentRepository(ETIDriveContext context) : base(context)
        {
            this.context = context;
        }

        public void AddUserToDepartment(User user, Department department)
        {
            user.DepartmentId = department.DepartmentId;
            user.Department = department;
            context.SaveChanges();
        }

        public void RemoveUserFromDepartment(User user, int departmentId)
        {
            user.DepartmentId = null;
            context.SaveChanges();
        }

        public async Task<List<User>> GetUserbyDepartment(int departmentId)
        {
            return await context.Users
                .Where(u => u.DepartmentId == departmentId)
                .ToListAsync();
        }

        public async Task<Folder?> GetDepartmentFolder(int departmentId)
        {
            return await context.Departments
                .Where(d => d.DepartmentId == departmentId)
                .Select(d => d.Folder)
                .FirstOrDefaultAsync();
        }

        public bool IsInDepartment(string userId, int departmentId)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null || !user.DepartmentId.HasValue)
            {
                return false;
            }

            return user.DepartmentId.Value == departmentId;
        }
    }
}
