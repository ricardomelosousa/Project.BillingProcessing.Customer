
namespace Project.BillingProcessing.Customer.Domain.Service.Interfaces
{
    public interface ICustomerService 
    {
       Task<int> Create(CustomerEntity.Customer customer);
        Task<CustomerEntity.Customer> FindBy(Expression<Func<CustomerEntity.Customer, bool>> where);

        Task<IEnumerable<CustomerEntity.Customer>> GetAll(int take);
    }
}
