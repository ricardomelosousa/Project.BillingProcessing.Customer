using Project.BillingProcessing.Customer.Infra.ConfigurationMap;

namespace Project.BillingProcessing.Customer.Infra
{
    public class CustomerContext : DbContext, IUnitOfWork
    {
        public virtual DbSet<Domain.CustomerEntity.Customer> Customers { get; set; }

        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerEntityConfigurationMap());
            base.OnModelCreating(modelBuilder);
        }
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
