namespace Project.BillingProcessing.Customer.Domain.CustomerEntity;
public interface ICustomerRepository : IRepository<Customer>
{
    void Create(Customer customer);
    Task<Customer> FindBy(Expression<Func<Customer, bool>> where);
}


