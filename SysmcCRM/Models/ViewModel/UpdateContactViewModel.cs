using SysmcCRM.Models.Domain;

namespace SysmcCRM.Models.ViewModel
{
    public class UpdateContactViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? OfficeNumber { get; set; }
        public string? Email { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }
    }
}
