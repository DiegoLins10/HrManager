using HrManager.Core.Entities;

namespace HrManager.Infrastructure.Persistence.Repositories
{
    public class EmployeeRepository
    {
        private readonly HrManagerDbContext _context;

        public EmployeeRepository(HrManagerDbContext context)
        {
            _context = context;
        }

        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);

            _context.SaveChanges();
        }
    }
}