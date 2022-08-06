using Project.BillingProcessing.Customer.Domain.CustomerEntity;
using Project.BillingProcessing.Customer.Domain.Service.Interfaces;

namespace Project.BillingProcessing.Customer.Domain.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRespository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRespository = customerRepository;
        }
        public async Task<int> Create(CustomerEntity.Customer customer)
        {
            try
            {
                _customerRespository.Create(customer);
                var customerId = await _customerRespository.UnitOfWork.SaveChangesAsync();
                return customerId;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        public async Task<CustomerEntity.Customer> FindBy(Expression<Func<CustomerEntity.Customer, bool>> where)
        {
            return await _customerRespository.FindBy(where);
        }
    }
}
