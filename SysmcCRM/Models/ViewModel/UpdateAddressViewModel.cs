using SysmcCRM.Models.Domain;

namespace SysmcCRM.Models.ViewModel
{
    public class UpdateAddressViewModel
    {
        public int Id { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }
    }
}
