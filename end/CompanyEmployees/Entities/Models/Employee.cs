using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Employee
    {
        [Column("EmployeeId")]
        public Guid Id { get; set; }
        [Column("Age")]
        public int Age { get; set; }
        [Column("CompanyId")]
        [ForeignKey("Company")]
        public Guid CompanyId { get; set; }       
        public virtual Company Company { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Position")]
        public string Position { get; set; }
        
        public virtual ICollection<Address> Addresses { get; set; }

    }
}
