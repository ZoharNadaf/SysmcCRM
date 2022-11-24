using SysmcCRM.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace SysmcCRM.Models.ViewModel
{
    public class UpdateCustomerViewModel
    {
        public UpdateCustomerViewModel()
        {
            Addresses = new HashSet<Address>();
            Contacts = new HashSet<Contact>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Customer Number should be a 9 digits number")]
        [RegularExpression(@"\d{9}", ErrorMessage = "Customer Number should be a 9 digits number")]
        public long CustomerNumber { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}
