using Moq;
using Project.BillingProcessing.Customer.Domain.CustomerEntity;
using Project.BillingProcessing.Customer.Domain.SeedWork;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Project.BillingProcessing.Customer.Api.Test
{
    public class CustomerControllerTest
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitMock;
        private List<Domain.CustomerEntity.Customer> Customers;

        public CustomerControllerTest()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _unitMock = new Mock<IUnitOfWork>();
            Customers = new List<Domain.CustomerEntity.Customer>()
            {
              new Domain.CustomerEntity.Customer(){ Id = 1, Identification = 12345678954, Name = "Ricardo 1"},
              new Domain.CustomerEntity.Customer(){ Id = 2, Identification = 12346678954, Name = "Ricardo 2"},
              new Domain.CustomerEntity.Customer(){ Id = 3, Identification = 37514289097, Name = "Ricardo 3"},
              new Domain.CustomerEntity.Customer(){ Id = 4, Identification = 15407855054, Name = "Ricardo 4"},
              new Domain.CustomerEntity.Customer(){ Id = 5, Identification = 62710176068, Name = "Ricardo 5"},
            };
        }

        [Fact]
        public async void Identification_Is_Valid()
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
        public async void Identification_Number_InValid(string identification)
        {
            Assert.Throws<Domain.Exceptions.CustomerException>(() => new Domain.CustomerEntity.Customer("Ricardo", "1", identification)); //customer.FormatIdentification("331.101.760-0"));
        }

        [Theory]
        [InlineData("375.142.890-97")]
        [InlineData("627.101.760-68")]
        [InlineData("154.078.550-54")]
        public async void Get_Customer_by_identification(string identification)
        {
            //A          
            var identificationFormated = new Domain.CustomerEntity.Customer().FormatIdentification(identification);
            _customerRepositoryMock.Setup(a => a.FindBy(a => a.Identification == identificationFormated)).ReturnsAsync(Customers.Where(a => a.Identification == identificationFormated).ToList());
            //A
            var listCustomers = await _customerRepositoryMock.Object.FindBy(a => a.Identification == identificationFormated);
            //A
            Assert.Equal(1, listCustomers?.Count());
        }

        [Fact]
        public async void Create_Customer()
        {
            //A
            var customer = new Domain.CustomerEntity.Customer("Ricardo", "1", "627.101.760-68");
            _customerRepositoryMock.Setup(a => a.Create(customer)).Returns(1);
            //A
           var customerId = _customerRepositoryMock.Object.Create(customer);
            //A
            Assert.Equal(1, customerId);
        }

    }
}