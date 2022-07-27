using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public int Age { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public List<AddressDto> Addresses { get; set; }
    }

    public class AddressDto
    {
        public int Id { get; set; }
        public int PostalCode { get; set; }
        public string Description { get; set; }
    }
}
