namespace Project.BillingProcessing.Customer.Domain.CustomerEntity;
public interface ICustomerRepository : IRepository<Customer>
{
    int Create(Customer customer);
    Task<IEnumerable<Customer>> FindBy(Expression<Func<Customer, bool>> where);
}


