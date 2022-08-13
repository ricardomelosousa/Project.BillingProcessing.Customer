using AutoMapper;
using Microsoft.Extensions.Logging;
using Project.BillingProcessing.Customer.Api.Controllers;
using Project.BillingProcessing.Customer.Api.Services;
using Project.BillingProcessing.Customer.Domain.Service.Interfaces;
using System.Threading.Tasks;
using GrpcCustomers;

namespace Project.BillingProcessing.Customer.Test;
public class CustomerScenariosTest : IClassFixture<CustomerScenariosBaseTest<Program>>
{
    private readonly Mock<ICustomerService> _customerServiceMock;
    private readonly Mock<IUnitOfWork> _unitMock;
    private readonly Mock<ILogger<CustomerGrpcService>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;

    private List<Domain.CustomerEntity.Customer> Customers;
    private readonly HttpClient _client;

    public CustomerScenariosTest(CustomerScenariosBaseTest<Program> factory)
    {
        _client = factory.CreateClient();
        _customerServiceMock = new Mock<ICustomerService>();
        _loggerMock = new Mock<ILogger<CustomerGrpcService>>();
        _unitMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        Customers = new List<Domain.CustomerEntity.Customer>()
            {
              new Domain.CustomerEntity.Customer(){ Id = 1, Identification = 12345678954, Name = "Ricardo 1"},
              new Domain.CustomerEntity.Customer(){ Id = 2, Identification = 12346678954, Name = "Ricardo 2"},
              new Domain.CustomerEntity.Customer(){ Id = 3, Identification = 37514289097, Name = "Ricardo 3"},
              new Domain.CustomerEntity.Customer(){ Id = 4, Identification = 15407855054, Name = "Ricardo 4"},
              new Domain.CustomerEntity.Customer(){ Id = 5, Identification = 62710176068, Name = "Ricardo 5"},
            };
    }

    [Theory]
    [InlineData("375.142.890-97")]
    [InlineData("627.101.760-68")]
    [InlineData("154.078.550-54")]
    public async void Test_Integration_Get_Customer_NotFound(string identification)
    {
        //A
        var response = await _client.GetAsync($"api/v1/customers/GetCustomerByIdentification?identification={identification}");
        var status = response.StatusCode;
        var responseString = await response.Content.ReadAsStringAsync();
        //A
        Assert.Equal(System.Net.HttpStatusCode.NotFound, status);
        Assert.Equal("Cliente não localizado", responseString);
    }

    [Theory]
    [InlineData("375.142.890-97")]
    [InlineData("627.101.760-68")]
    [InlineData("154.078.550-54")]
    public async void Test_Integration_Get_Customer_Success(string identification)
    {
        //A
        var identificationFormated = new Domain.CustomerEntity.Customer().FormatIdentification(identification);
        _customerServiceMock.Setup(a => a.FindBy(a => a.Identification == It.IsAny<long>())).ReturnsAsync(Customers.Where(a => a.Identification == identificationFormated).FirstOrDefault());

        //A
        var controller = new CustomersController(_customerServiceMock.Object);
        var response = await _client.GetAsync($"api/v1/customers/GetCustomerByIdentification?identification={identification}");
        var status = response.StatusCode;
        var responseString = await response.Content.ReadAsStringAsync();
        //A
        Assert.Equal(System.Net.HttpStatusCode.NotFound, status);
        Assert.Equal("Cliente não localizado", responseString);
    }


    [Fact]
    public async void Test_Integration_Create_Customer_Success()
    {
        //A
        var data = new CustomerDto() { Name = "Ricardo 12", State = "1", Identification = "627.101.760-68" };
        var jsonContent = JsonSerializer.Serialize(data);
        var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //A
        HttpResponseMessage response2 = await _client.PostAsync("api/v1/customers/createCustomer", contentString);
        //A
        Assert.Equal(System.Net.HttpStatusCode.OK, response2.StatusCode);

    }

    [Fact]
    public async void Test_Integration_Create_Customer_BadRequest()
    {
        //A
        var data = new CustomerDto() { Name = "Ricardo 12", State = "1", Identification = "627.101.7" };
        var jsonContent = JsonSerializer.Serialize(data);
        var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //A
        HttpResponseMessage response2 = await _client.PostAsync("api/v1/customers/createCustomer", contentString);
        //A
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response2.StatusCode);

    }

    [Fact]
    public async void Test_Identification_Is_Valid()
    {
        //A
        var identification = "627.101.760-68";

        //A
        var customer = new Domain.CustomerEntity.Customer("Ricardo", "1", identification);// new Domain.CustomerEntity.Customer().FormatIdentification(identification);        


        //A
        Assert.Equal(62710176068, customer.Identification);

    }

    [Theory]
    [InlineData("375.142.890-9")]
    [InlineData("627.101.760")]
    [InlineData("154.078.55")]
    [InlineData("")]
    [InlineData(null)]
    public async void Test_Identification_Number_InValid(string identification)
    {
        Assert.Throws<Domain.Exceptions.CustomerException>(() => new Domain.CustomerEntity.Customer("Ricardo", "1", identification)); //customer.FormatIdentification("331.101.760-0"));
    }

    [Theory]
    [InlineData("375.142.890-97")]
    [InlineData("627.101.760-68")]
    [InlineData("154.078.550-54")]
    public async void Test_Get_Customer_by_identification(string identification)
    {
        //A          
        var identificationFormated = new Domain.CustomerEntity.Customer().FormatIdentification(identification);
        _customerServiceMock.Setup(a => a.FindBy(a => a.Identification == identificationFormated)).ReturnsAsync(Customers.Where(a => a.Identification == identificationFormated).FirstOrDefault());
        //A
        var customer = await _customerServiceMock.Object.FindBy(a => a.Identification == identificationFormated);
        //A
        Assert.NotNull(customer);
    }

    [Fact]
    public async void Test_Create_Customer()
    {
        //A
        var customer = new Domain.CustomerEntity.Customer("Ricardo", "1", "627.101.760-68");
        _customerServiceMock.Setup(a => a.Create(customer)).ReturnsAsync(1);
        //A
        var customerId = await _customerServiceMock.Object.Create(customer);
        //A
        Assert.Equal(1, customerId);
    }

    [Theory]
    [InlineData("375.142.890-97")]
    [InlineData("627.101.760-68")]
    [InlineData("154.078.550-54")]
    public async Task Test_Show_Customer_By_grpc(string identification)
    {
        // A       
        var identificationFormated = new Domain.CustomerEntity.Customer().FormatIdentification(identification);
        _customerServiceMock.Setup(a => a.FindBy(a => a.Identification == identificationFormated)).ReturnsAsync(Customers.Where(a => a.Identification == identificationFormated).FirstOrDefault());
        var service = new CustomerGrpcService(_customerServiceMock.Object, _mapperMock.Object, _loggerMock.Object);
        // A
        var response = await service.GetCustomerByIdentification(new GrpcCustomers.GetCustomerByIdentificationRequest { Identification = identification }, null);//TestServerCallContext.Create());
        // A        
        Assert.Equal(Customers.Where(a => a.Identification == identificationFormated).FirstOrDefault(), _mapperMock.Object.Map<Customer.Domain.CustomerEntity.Customer>(response));
    }


    [Fact]
    public async Task Test_Create_Customer_By_grpc()
    {
        //A
        var customer = new Domain.CustomerEntity.Customer("Ricardo", "1", "627.101.760-68");
        _customerServiceMock.Setup(a => a.Create(customer)).ReturnsAsync(1);
        var service = new CustomerGrpcService(_customerServiceMock.Object, _mapperMock.Object, _loggerMock.Object);
        _customerServiceMock.Setup(a => a.Create(customer)).ReturnsAsync(1);

        //A
        var customerId = await _customerServiceMock.Object.Create(customer);
        var customerModelRequest = _mapperMock.Object.Map<CustomerModelRequest>(customer);
        var response = await service.CreateCustomer(customerModelRequest, null);// TestServerCallContext.Create());
        //A
        Assert.Equal(customerId, response.Id);
    }

}
