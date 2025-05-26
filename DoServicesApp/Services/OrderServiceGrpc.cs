using DoServicesApp.Data;
using DoServicesApp.Models;
using Grpc.Core;

namespace DoServicesApp.Services
{
    public class OrderServiceGrpc : OrderGrpc.OrderGrpcBase
    {

        private readonly OrderDbContext _context;

        public OrderServiceGrpc(OrderDbContext context)
        {
            _context = context;
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
    }
}
