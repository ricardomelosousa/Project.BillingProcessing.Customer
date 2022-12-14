using Project.BillingProcessing.Customer.Api.Services;

var builder = WebApplication.CreateBuilder(args);
var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var conSqlServer = builder.Configuration.GetConnectionString("ConnectionStringSql");
builder.Services.AddDbContext<CustomerContext>(options => options.UseSqlServer(conSqlServer, options => options.EnableRetryOnFailure()));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.MapGrpcService<CustomerGrpcService>().RequireHost("*:5001");
app.UseAuthorization();

app.MapControllers();

public partial class Program { }