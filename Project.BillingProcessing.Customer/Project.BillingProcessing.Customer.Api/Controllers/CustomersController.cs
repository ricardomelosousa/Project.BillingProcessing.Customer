using Microsoft.AspNetCore.Mvc;
using Project.BillingProcessing.Customer.Api.Dto;
using System.Net;

namespace Project.BillingProcessing.Customer.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
       

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
          
        }


        [Route("GetCustomerByIdentification")]
        [HttpGet]
        [ProducesResponseType(typeof(Domain.CustomerEntity.Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetCustomerByIdentification(string identification)
        {
            try
            {
                var identificationFormated = new Domain.CustomerEntity.Customer().FormatIdentification(identification);
                var customer = await _customerService.FindBy(a => a.Identification == identificationFormated);
                if (customer == null)
                    return NotFound("Cliente não localizado");
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [Route("createCustomer")]
        [HttpPost]
        [ProducesResponseType(typeof(StatusCodeResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            try
            {
                var identificationFormated = new Domain.CustomerEntity.Customer().FormatIdentification(customerDto.Identification);
                var customer = await _customerService.FindBy(a => a.Identification == identificationFormated);
                if (customer != null)
                    return BadRequest($"Cliente com cpf {customerDto.Identification} já cadastrado anteriormente.");
                var result = _customerService.Create(new Domain.CustomerEntity.Customer(customerDto.Name, customerDto.State, customerDto.Identification));
                return Ok();
            }
            catch (Exception ex)
            {
                //_logger.LogCritical("Error in CreateCustomer ", customerDto, ex.Message);
                return BadRequest(ex.Message);
            }

        }
    }
}
