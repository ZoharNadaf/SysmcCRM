 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SysmcCRM.Data;
using SysmcCRM.Models.Domain;
using SysmcCRM.Models.ViewModel;
using System.Text.Json;

namespace SysmcCRM.Controllers
{
    public class CustomersController : Controller
    {
        private readonly SysmcDbCrmContext sysmcDbCrmContext;

        public CustomersController(SysmcDbCrmContext sysmcDbCrmContext)
        {
            this.sysmcDbCrmContext = sysmcDbCrmContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var customers = await sysmcDbCrmContext.Customers.Where(x => x.IsDeleted == false).ToListAsync();
            return View(customers);
        }

        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(AddCustomerViewModel addCustomerRequest)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer()
                {
                    Name = addCustomerRequest.Name,
                    CustomerNumber = addCustomerRequest.CustomerNumber,
                    IsDeleted = false,
                    Created = DateTime.Now
                };
                await sysmcDbCrmContext.Customers.AddAsync(customer);
                await sysmcDbCrmContext.SaveChangesAsync();
                return Redirect("Index");
            }
            else
            {
                return await Task.Run(() => View("AddCustomer", addCustomerRequest));
            }         
        }

        [HttpPost]
        public JsonResult CheckIfCustomerExist(IFormCollection formcollection)
        {
            JsonResponseViewModel model = new JsonResponseViewModel();
            string formName = formcollection["name"].ToString();
            if (!string.IsNullOrWhiteSpace(formName))
            {
                var customerCheckName = sysmcDbCrmContext.Customers.Where(x => x.Name == formName).FirstOrDefault();
                if (customerCheckName != null)
                {
                    model.ResponseCode = 0;
                    model.ResponseMessage = "This Name already exist in customer list";
                }
                else
                {
                    model.ResponseCode = 1;
                    //model.ResponseMessage = "";
                }
            }
            
            return Json(model);
        }

        [HttpGet]
        public async Task<IActionResult> CustomerDetails(int id)
        {
            var customer = await sysmcDbCrmContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (customer != null)
            {
                var viewCustomer = new UpdateCustomerViewModel()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Created = customer.Created,
                    CustomerNumber = customer.CustomerNumber,
                    IsDeleted = customer.IsDeleted,
                    Addresses = await sysmcDbCrmContext.Addresses.Where(x => x.IsDeleted == false && x.CustomerId == customer.Id).ToListAsync(),
                    Contacts = await sysmcDbCrmContext.Contacts.Where(x => x.IsDeleted == false && x.CustomerId == customer.Id).ToListAsync()
                };
                return await Task.Run(() => View("CustomerDetails", viewCustomer));
            }
            return Redirect("/Home/Error"); // add an errror 
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomerDetails(UpdateCustomerViewModel updateCustomer)
        {
            if (ModelState.IsValid)
            {
                var customer = await sysmcDbCrmContext.Customers.FindAsync(updateCustomer.Id);
                if (customer != null && !string.IsNullOrWhiteSpace(updateCustomer.Name) && updateCustomer.CustomerNumber != 0)
                {
                    customer.CustomerNumber = updateCustomer.CustomerNumber;
                    customer.Name = updateCustomer.Name;
                    await sysmcDbCrmContext.SaveChangesAsync();
                    return await Task.Run(() => View("CustomerDetails", updateCustomer));
                }
                return Redirect("/Home/Error"); // add an errror 
            }
            return await Task.Run(() => View("CustomerDetails", updateCustomer));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateCustomerViewModel updateCustomer)
        {
            var customer = await sysmcDbCrmContext.Customers.FindAsync(updateCustomer.Id);
            if (customer != null)
            {
                customer.IsDeleted = true;
                await sysmcDbCrmContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return Redirect("/Home/Error"); // add an errror 
        }
    }
}
