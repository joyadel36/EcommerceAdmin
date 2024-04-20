using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class CustomersServices : ICustomersRepo
    {
        public ECommerceContext _Context { get; set; }
        public CustomersServices(ECommerceContext context)
        {
            _Context = context;
        }

        public bool CreateCustomer(Customers CCustomer)
        {
            if (CCustomer != null)
            {
                _Context.Customers.Add(CCustomer);
                _Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteCustomer(int CID)
        {
            Customers? tempCustomer = _Context.Customers.Where(C => C.ID == CID).FirstOrDefault();
            if (tempCustomer != null)
            {
                _Context.Customers.Remove(tempCustomer);
                _Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EditCustomer(int CID, Customers ECustomers)
        {
            Customers? tempCustomer = _Context.Customers.Where(C => C.ID == CID).FirstOrDefault();
            if (tempCustomer != null)
            {
                tempCustomer.FirstName= ECustomers.FirstName;
                tempCustomer.LastName = ECustomers.LastName;
                tempCustomer.PhoneNumber = ECustomers.PhoneNumber;
                tempCustomer.Email = ECustomers.Email;
                tempCustomer.Street = ECustomers.Street;
                tempCustomer.City = ECustomers.City;
                tempCustomer.BuildingNumber = ECustomers.BuildingNumber;
                tempCustomer.Region = ECustomers.Region;
                _Context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Customers>? GetAllCustomers()
        {
            return _Context.Customers.ToList();
        }

        public Customers? GetCustomerByID(int CID)
        {
            return _Context.Customers.Where(c=>c.ID==CID).FirstOrDefault();
        }

        public List<Customers>? SearchByName(string FName)
        {
            return _Context.Customers.Where(c => c.FirstName == FName || c.LastName == FName).ToList();

        }
    }
}
