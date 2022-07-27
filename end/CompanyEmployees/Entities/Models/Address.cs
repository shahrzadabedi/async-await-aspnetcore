using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int PostalCode { get; set; }
        public string Description { get; set; }     
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

    }
}
