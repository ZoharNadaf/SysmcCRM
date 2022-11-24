using System.ComponentModel.DataAnnotations;

namespace SysmcCRM.Models.ViewModel
{
    public class AddCustomerViewModel
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Customer Number should be a 9 digits number")]
        //[Range(100000000,999999999, ErrorMessage = "Customer Number should be a 9 digits number")]
        [RegularExpression(@"\d{9}", ErrorMessage = "Customer Number should be a 9 digits number")]
        public long CustomerNumber { get; set; }
    }
}
