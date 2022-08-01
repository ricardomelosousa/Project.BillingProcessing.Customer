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

        public void Create(Domain.CustomerEntity.Customer customer)
        {
            _customerContext.Add(customer);
           
        }

        public async Task<IList<Domain.CustomerEntity.Customer>> FindBy(Expression<Func<Domain.CustomerEntity.Customer, bool>> where)
        {
           return await _customerContext.Set<Domain.CustomerEntity.Customer>().Where(where).ToListAsync();
        }
    }
}
