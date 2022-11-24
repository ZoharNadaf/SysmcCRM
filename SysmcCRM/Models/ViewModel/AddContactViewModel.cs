using System;
using System.Collections.Generic;

namespace SysmcCRM.Models.ViewModel
{
    public partial class AddContactViewModel
    {
        public string FullName { get; set; } = null!;
        public string? OfficeNumber { get; set; }
        public string? Email { get; set; }
    }
}
