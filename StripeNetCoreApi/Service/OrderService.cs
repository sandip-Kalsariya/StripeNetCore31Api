using Microsoft.EntityFrameworkCore;
using StripeNetCoreApi.Controllers.Base;
using StripeNetCoreApi.DTO.EnumDTO;
using StripeNetCoreApi.DTO.RequestDTO;
using StripeNetCoreApi.DTO.ResponseDTO;
using StripeNetCoreApi.Entity;
using StripeNetCoreApi.Model;
using StripeNetCoreApi.Repository.IRepository;
using StripeNetCoreApi.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Service
{
    public class OrderService : BasicService, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IStripeDbContext _context;
        public OrderService(
            IOrderRepository orderRepository,
            IStripeDbContext context
            )
        {
            _orderRepository = orderRepository;
            _context = context;
        }
        public async Task<Response<Order>> Create(int UserId, OrderRequestDTO dto)
        {
            var response = new Response<Order>();
            double discountamount = 0;
            double rate = 0;
            try
            {
                Order order = new Order();
                order.UserId = UserId;
                order.Description = dto.Notes;

                if (dto.DiscountCode != "")
                {
                    if (dto.DiscountType == DiscountType.Price)
                    {
                        discountamount = dto.DiscountAmount;
                    }
                    else if (dto.DiscountType == DiscountType.Percentage)
                    {
                        discountamount = dto.Price * dto.DiscountAmount / 100;
                    }
                    if (discountamount != dto.Discount)
                    {
                        response.AddValidationError("", " discount does not match with request parameters values.");
                        return response;
                    }
                    order.DiscountCode = dto.DiscountCode;
                    order.Discount = discountamount;
                    order.DiscountType = dto.DiscountType;
                }

                order.Price = dto.Price;
                order.Fee = ((order.Price - discountamount) * rate / 100);
                order.Net = ((order.Price - discountamount) - order.Fee);

                // Charges for influencer
                double amt = order.Net + order.Fee;
                double centamt = amt * 100;

                double Commission = (centamt * rate / 100); // give payout to influence commision rate = 15%
                double fees = centamt - Commission;   // payable amount = (service price  - (commision rate % of service price))
                double charge = (fees * 2.9 / 100);    // 2.9 % of service price
                double fixchargeincent = 0.30 * 100;   // 0.30 Fixed Charge
                double totalCharg = charge + fixchargeincent;
                double finalAmount = (fees - totalCharg);

                double cent50 = (1 * 100) / 2;

                if (finalAmount < cent50)
                {
                    response.AddValidationError("", " Can not create order.");
                    return response;
                }
                order.Charge = totalCharg / 100;
                order.Status = OrderStatus.RequestPending;
                order.DateCreated = DateTime.UtcNow.ToString();
                _orderRepository.Insert(order);

                response.Success = true;
                response.Data = order;
            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }
        public Response<Result<ResponseOrderDTO>> GetUserOrderList(int UserId, Pagination dto)
        {
            var response = new Response<Result<ResponseOrderDTO>>();
            List<ResponseOrderDTO> requestedOrderResponsesDTO = new List<ResponseOrderDTO>();
            try
            {
                var requestedOrders = _context.Orders.Where(a => a.UserId == UserId && a.Status == OrderStatus.RequestPending && a.DateDeleted == null).ToList();

                if (requestedOrders.Count != 0)
                {
                    var data = requestedOrders.Skip((dto.Page - 1) * dto.PageSize).Take(dto.PageSize);
                    foreach (var item in data)
                    {
                        ResponseOrderDTO requestedOrderResponse = new ResponseOrderDTO();
                        requestedOrderResponse.OrderId = item.Id;
                        requestedOrderResponse.CreatedDate = item.DateCreated;
                        requestedOrderResponse.Amount = (item.Price - item.Discount);
                        requestedOrderResponse.Discount = item.Discount;
                        requestedOrderResponse.Status = new ResponseOrderStatus { Id = item.Status, Name = Enum.GetName(typeof(OrderStatus), item.Status) };
                        requestedOrderResponse.PaymentStaus = (item.PaymentStaus != null && item.PaymentStaus > 0) ? new ResponsePaymentStatus { Id = item.PaymentStaus, Name = Enum.GetName(typeof(PaymentStaus), item.PaymentStaus) } : null;
                        requestedOrderResponsesDTO.Add(requestedOrderResponse);
                    }
                }
                var resdata = new Result<ResponseOrderDTO>
                {
                    Data = requestedOrderResponsesDTO,
                    Count = requestedOrders.Count
                };
                response.Data = resdata;
                response.Success = true;
            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }
    }
}
