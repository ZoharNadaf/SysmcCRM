using System;
using System.Collections.Generic;

namespace SysmcCRM.Models.Domain
{
    public partial class Customer
    {
        public Customer()
        {
            Addresses = new HashSet<Address>();
            Contacts = new HashSet<Contact>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public long CustomerNumber { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
