using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly RepositoryContext _repoContext;
        public EmployeeRepository(RepositoryContext repoContext)
        {
            _repoContext = repoContext;
        }
        public async Task CreateEmployee(Employee employee)
        {
            _repoContext.Add(employee);
            await _repoContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _repoContext.Employees
                 .OrderBy(c => c.Id)
                 .ToListAsync();
        }


        public async Task<Employee> GetEmployee(Guid Id)
        {
            var result = await _repoContext.Employees
                   .SingleOrDefaultAsync(c => c.Id.Equals(Id));
           
            return result;
        }


    }
}
