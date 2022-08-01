

namespace Project.BillingProcessing.Customer.Domain.CustomerEntity
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Customer CreateCustomer(Customer customer);
        IList<Customer> FindBy(Expression<Func<Customer, bool>> where);
    }
}

