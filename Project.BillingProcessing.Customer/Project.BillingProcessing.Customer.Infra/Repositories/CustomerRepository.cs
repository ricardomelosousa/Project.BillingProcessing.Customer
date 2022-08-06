namespace Project.BillingProcessing.Customer.Infra.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext _customerContext;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _customerContext;
            }
        }

        public CustomerRepository(CustomerContext customerContext)
        {
            _customerContext = customerContext;
        }

        public async Task Create(Domain.CustomerEntity.Customer customer)
        {
            await _customerContext.Customers.AddAsync(customer);
          
        }

        public async Task<Domain.CustomerEntity.Customer> FindBy(Expression<Func<Domain.CustomerEntity.Customer, bool>> where) 
            => await _customerContext.Set<Domain.CustomerEntity.Customer>().Where(where).FirstOrDefaultAsync();
    }
}
