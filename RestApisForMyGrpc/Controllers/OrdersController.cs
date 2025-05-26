using DoServicesApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;

namespace RestApisForMyGrpc.Controllers
{

    public class OrdersController : BaseController
    {

        private readonly OrderGrpc.OrderGrpcClient _orderGrpcClient;
        public OrdersController(OrderGrpc.OrderGrpcClient orderGrpcClient)
        {

            _orderGrpcClient = orderGrpcClient;

        }


        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateNewOrder([FromBody] CreateOrderRequest request)
        {
            var response = await _orderGrpcClient.CreateOrderAsync(request);

            if (response == null)
            {
                return BadRequest("Failed to create order.");
            }

            return Ok(response);
        }


        [HttpGet("GetOrder/{id}")]
        public async Task<IActionResult> GetOrder(string id)
        {
            var request = new GetOrderRequest { OrderId = id };
            var response = await _orderGrpcClient.GetOrderAsync(request);
            if (response == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }
            return Ok(response);
        }

        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _orderGrpcClient.GetOrdersAsync(new Google.Protobuf.WellKnownTypes.Empty());
            if (response == null || response.Orders.Count == 0)
            {
                return NotFound("No orders found.");
            }
            return Ok(response);
        }
    }
}
