namespace ETIDrive_xUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void AddUserToDepartment_ShouldSetDepartmentIdAndDepartment()
        {
            var user = new User();
            Department department = new Department
            {
                DepartmentId = 1,
                Name = "My Department"
            };

            static void AddUserToDepartment(User user, Department department)
            {
                user.DepartmentId = department.DepartmentId;
                user.Department = department;
            }

            // Act
            AddUserToDepartment(user, department);

            // Assert
            Assert.Equal(department.DepartmentId, user.DepartmentId);
            Assert.Equal(department, user.Department);
        }
    }
}