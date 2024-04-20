using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface ICustomersRepo
    {
        public List<Customers>? GetAllCustomers();
        public List<Customers>? SearchByName(string FName);
        public Customers? GetCustomerByID(int CID);
        public bool CreateCustomer(Customers CCustomer);
        public bool EditCustomer(int CID, Customers ECustomers);
        public bool DeleteCustomer(int CID);
    }
}
