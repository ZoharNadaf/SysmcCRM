 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SysmcCRM.Data;
using SysmcCRM.Models.Domain;
using SysmcCRM.Models.ViewModel;

namespace SysmcCRM.Controllers
{
    public class AddressesController : Controller
    {
        private readonly SysmcDbCrmContext sysmcDbCrmContext;

        public AddressesController(SysmcDbCrmContext sysmcDbCrmContext)
        {
            this.sysmcDbCrmContext = sysmcDbCrmContext;
        }

        [HttpGet]
        public IActionResult AddAddress(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(AddAddressViewModel addAddressRequest)
        {
            var path = this.HttpContext.Request.Path.Value;
            int customerId = 0;
            if (path !=null &&
                int.TryParse(path.Remove(0, path.LastIndexOf('/') + 1), out customerId) && 
                !string.IsNullOrWhiteSpace(addAddressRequest.City) && 
                !string.IsNullOrWhiteSpace(addAddressRequest.Street))
            {
                var customer = await sysmcDbCrmContext.Customers.FirstOrDefaultAsync(x => x.Id == customerId);
                var address = new Address()
                {
                    CustomerId = customerId,
                    City = addAddressRequest.City,
                    Street = addAddressRequest.Street,
                    IsDeleted = false,
                    Created = DateTime.Now
                };
                await sysmcDbCrmContext.Addresses.AddAsync(address);
                await sysmcDbCrmContext.SaveChangesAsync();

                return Redirect("/Customers/CustomerDetails/" + customerId);
            }
            else
                return Redirect("Index");
        }

        [HttpGet]
        public async Task<IActionResult> AddressDetails(int id)
        {
            var address = await sysmcDbCrmContext.Addresses.FirstOrDefaultAsync(x => x.Id == id);
            if (address != null)
            {
                var viewAddress = new UpdateAddressViewModel()
                {
                    Id = address.Id,
                    City = address.City,
                    Street = address.Street,
                    Created = address.Created,
                    IsDeleted = address.IsDeleted,
                };
                return await Task.Run(() => View("AddressDetails", viewAddress));
            }
            return Redirect("/Home/Error"); // add an errror 
        }

        [HttpPost]
        public async Task<IActionResult> AddressDetails(UpdateAddressViewModel updateAddress)
        {
            var address = await sysmcDbCrmContext.Addresses.FindAsync(updateAddress.Id);
            if (address != null &&
                !string.IsNullOrWhiteSpace(updateAddress.City) &&
                !string.IsNullOrWhiteSpace(updateAddress.Street))
            {
                address.City = updateAddress.City;
                address.Street = updateAddress.Street;
                await sysmcDbCrmContext.SaveChangesAsync();
                return Redirect("/Customers/CustomerDetails/" + address.CustomerId);
            }
            return Redirect("/Home/Error"); // add an errror 
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateAddressViewModel updateAddress)
        {
            var address = await sysmcDbCrmContext.Addresses.FindAsync(updateAddress.Id);
            if (address != null)
            {
                address.IsDeleted = true;
                await sysmcDbCrmContext.SaveChangesAsync();
                return Redirect("/Customers/CustomerDetails/" + address.CustomerId);
            }
            return Redirect("/Home/Error"); // add an errror 
        }
    }
}
