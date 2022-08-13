using Project.BillingProcessing.Customer.Api.Services;

namespace Project.BillingProcessing.Customer.Api;
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<CustomerContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("ConnectionStringSql")));

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddGrpc();
        services.AddAutoMapper(typeof(Startup));

        services.AddControllersWithViews();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<CustomerGrpcService>();//.RequireHost("*:5001");
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Employees}/{action=Index}/{id?}");
        });
    }
}

