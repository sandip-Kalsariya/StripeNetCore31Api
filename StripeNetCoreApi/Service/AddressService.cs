using StripeNetCoreApi.Controllers.Base;
using StripeNetCoreApi.DTO.ResponseDTO;
using StripeNetCoreApi.Entity;
using StripeNetCoreApi.Repository.IRepository;
using StripeNetCoreApi.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Service
{
    public class AddressService : BasicService, IAddressSerevice
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }
        public Response<Address> Create(Address dto)
        {
            var response = new Response<Address>();
            try
            {
                dto.DateCreated = DateTime.UtcNow.ToString();
                var res = _addressRepository.Insert(dto);
                if (res > 0)
                {
                    response.Data = dto;
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }
    }
}
