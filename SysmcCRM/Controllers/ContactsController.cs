 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SysmcCRM.Data;
using SysmcCRM.Models.Domain;
using SysmcCRM.Models.ViewModel;

namespace SysmcCRM.Controllers
{
    public class ContactsController : Controller
    {
        private readonly SysmcDbCrmContext sysmcDbCrmContext;

        public ContactsController(SysmcDbCrmContext sysmcDbCrmContext)
        {
            this.sysmcDbCrmContext = sysmcDbCrmContext;
        }

        [HttpGet]
        public IActionResult AddContact(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactViewModel addContactRequest)
        {
            var path = this.HttpContext.Request.Path.Value;
            int customerId = 0;
            if (path !=null &&
                int.TryParse(path.Remove(0, path.LastIndexOf('/') + 1), out customerId) && 
                !string.IsNullOrWhiteSpace(addContactRequest.FullName))
            {
                var customer = await sysmcDbCrmContext.Customers.FirstOrDefaultAsync(x => x.Id == customerId);
                var contact = new Contact()
                {
                    CustomerId = customerId,
                    FullName = addContactRequest.FullName,
                    OfficeNumber = addContactRequest.OfficeNumber,
                    Email = addContactRequest.Email,
                    IsDeleted = false,
                    Created = DateTime.Now
                };
                await sysmcDbCrmContext.Contacts.AddAsync(contact);
                await sysmcDbCrmContext.SaveChangesAsync();

                return Redirect("/Customers/CustomerDetails/" + customerId);
            }
            else
                return Redirect("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ContactDetails(int id)
        {
            var contact = await sysmcDbCrmContext.Contacts.FirstOrDefaultAsync(x => x.Id == id);
            if (contact != null)
            {
                var viewContact = new UpdateContactViewModel()
                {
                    Id = contact.Id,
                    FullName = contact.FullName,
                    OfficeNumber = contact.OfficeNumber,
                    Email = contact.Email,
                    IsDeleted = contact.IsDeleted,
                };
                return await Task.Run(() => View("ContactDetails", viewContact));
            }
            return Redirect("/Home/Error"); // add an errror 
        }

        [HttpPost]
        public async Task<IActionResult> ContactDetails(UpdateContactViewModel updateContact)
        {
            var contact = await sysmcDbCrmContext.Contacts.FindAsync(updateContact.Id);
            if (contact != null &&
                !string.IsNullOrWhiteSpace(updateContact.FullName))
            {
                contact.FullName = updateContact.FullName;
                contact.OfficeNumber = updateContact.OfficeNumber;
                contact.Email = updateContact.Email;
                await sysmcDbCrmContext.SaveChangesAsync();
                return Redirect("/Customers/CustomerDetails/" + contact.CustomerId);
            }
            return Redirect("/Home/Error"); // add an errror 
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateContactViewModel updateContact)
        {
            var contact = await sysmcDbCrmContext.Contacts.FindAsync(updateContact.Id);
            if (contact != null)
            {
                contact.IsDeleted = true;
                await sysmcDbCrmContext.SaveChangesAsync();
                return Redirect("/Customers/CustomerDetails/" + contact.CustomerId);
            }
            return Redirect("/Home/Error"); // add an errror 
        }
    }
}
