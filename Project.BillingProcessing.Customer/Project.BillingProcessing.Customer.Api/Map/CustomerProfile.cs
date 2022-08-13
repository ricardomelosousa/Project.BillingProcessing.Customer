using AutoMapper;
using GrpcCustomers;

namespace Project.BillingProcessing.Customer.Api.Map
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Domain.CustomerEntity.Customer, CustomerModelResponse>().ReverseMap();
            
        }
    }
}
