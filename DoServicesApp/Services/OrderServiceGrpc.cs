using DoServicesApp.Data;
using DoServicesApp.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace DoServicesApp.Services
{
    public class OrderServiceGrpc : OrderGrpc.OrderGrpcBase
    {

        private readonly OrderDbContext _context;

        public OrderServiceGrpc(OrderDbContext context)
        {
            _context = context;
        }


        public override async Task<OrderListResponse> GetOrders(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        {
            var orders = await _context.Orders.ToListAsync();
            var orderResponses = orders.Select(order => new OrderResponse
            {
                OrderId = order.Id.ToString(),
                UserId = order.UserId,
                ServiceId = order.ServiceId,
                Notes = order.Notes,
                Status = order.Status,
                CreatedAt = order.CreatedAt.ToString("O") // Format timestamp to ISO 8601
            }).ToList();

            return new OrderListResponse
            {
                Orders = { orderResponses } // Add the list to the repeated field
            };

        }

        public override async Task<OrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
        {
            var order = new Models.Order
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ServiceId = request.ServiceId,
                Notes = request.Notes,
                Status = "Pending"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return new OrderResponse
            {
                OrderId = order.Id.ToString(),
                UserId = order.UserId,
                ServiceId = order.ServiceId,
                Notes = order.Notes,
                Status = order.Status,
                CreatedAt = order.CreatedAt.ToString("O")
            };
        }

        public override async Task<OrderResponse> GetOrder(GetOrderRequest request, ServerCallContext context)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id.ToString() == request.OrderId);

            if (order == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Order not found"));
            }

            return new OrderResponse
            {
                OrderId = order.Id.ToString(),
                UserId = order.UserId,
                ServiceId = order.ServiceId,
                Status = order.Status,
                Notes = order.Notes,
                CreatedAt = order.CreatedAt.ToString("o")
            };
        }



    }
}
