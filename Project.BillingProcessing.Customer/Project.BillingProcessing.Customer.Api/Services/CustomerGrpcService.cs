using AutoMapper;
using Grpc.Core;
using Project.BillingProcessing.Customer.Api.Photos;

namespace Project.BillingProcessing.Customer.Api.Services
{
    public class CustomerGrpcService : CustomerProtoService.CustomerProtoServiceBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerGrpcService> _logger;
        private readonly IMapper _mapper;
        public CustomerGrpcService(ICustomerService customerService, IMapper mapper, ILogger<CustomerGrpcService> logger)
        {
            _customerService = customerService;
            _mapper = mapper;
            _logger = logger;
        }


        public override async Task<CustomerModelResponse> GetCustomerByIdentification(GetCustomerByIdentificationRequest request, ServerCallContext context)
        {
            var identificationFormated = new Domain.CustomerEntity.Customer().FormatIdentification(request.Identification);
            var customer = await _customerService.FindBy(a => a.Identification == identificationFormated);
            if (customer == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Cliente {request.Identification} não localizado"));
            }
            return _mapper.Map<CustomerModelResponse>(customer);
        }

        public override async Task<CreateCustomerModelResponse> CreateCustomer(CustomerModelRequest request, ServerCallContext context)
        {
            var identificationFormated = new Domain.CustomerEntity.Customer().FormatIdentification(request.Identification);
            var customer = await _customerService.FindBy(a => a.Identification == identificationFormated);
            if (customer != null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Cliente com cpf {request.Identification} já cadastrado anteriormente."));
            var result = await _customerService.Create(_mapper.Map<Domain.CustomerEntity.Customer>(customer));
            return new CreateCustomerModelResponse { Id = result};
        }
    }
}
