
namespace Project.BillingProcessing.Customer.Test.Services;
public class CustomerScenariosBaseTest<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<CustomerContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<CustomerContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryCustomeTest");
            });

            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            using (var appContext = scope.ServiceProvider.GetRequiredService<CustomerContext>())
            {
                try
                {
                    appContext.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                        //Log errors or do anything you think it's needed
                        throw;
                }
            }
        });


    }    
}

