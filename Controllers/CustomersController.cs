using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {

        private readonly ICustomersRepo _Customers;

        public CustomersController(ICustomersRepo customers)
        {
            _Customers = customers;

        }

    

        public IActionResult GetAllCustomers()
        {
           
            return View(_Customers.GetAllCustomers());

        }

        [HttpPost]
        public IActionResult GetAllCustomers(string query)
        { 
            List<Customers>? customers =_Customers.SearchByName(query.Split(" ")[0]);   
            return View(customers);
        }

        [HttpGet]
        public IActionResult GetCustomerById(int Id)
        {
            return View(_Customers.GetCustomerByID(Id));
        }

       
        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View(new Customers());
        }
        [HttpPost]
        public IActionResult CreateCustomer(Customers NewCustomer)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    bool createSuccess = _Customers.CreateCustomer(NewCustomer);
                    if (createSuccess)
                    {

                        return RedirectToAction("GetAllCustomers");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "An Error Occurred, Please try creating the category again..!");
                        return View();
                    }

                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        public IActionResult EditCustomer(int Id)
        {
            return View(_Customers.GetCustomerByID(Id));
        }
        [HttpPost]
        public IActionResult EditCustomer(int Id, Customers EditCustomer)
        {

            if (ModelState.IsValid)
            {
                try
                {


                    bool EditSuccess = _Customers.EditCustomer(Id, EditCustomer);
                    if (EditSuccess)
                    {
                        return RedirectToAction("GetAllCustomers");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "An Error Occurred, Please try Editing the category again..!");
                        return View();
                    }

                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }


        public IActionResult DeleteCustomer(int Id)
        {

            bool DeleteSuccess = _Customers.DeleteCustomer(Id);
            if (DeleteSuccess)
            {
                return RedirectToAction("GetAllCustomers");
            }
            else
            {
                ModelState.AddModelError("Error", "An Error Occurred, Please try Deleting the category again..!");
                return RedirectToAction("GetAllCustomers");
            }

        }

        
    }
}

