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
               
            if(response == null)
            {
                return BadRequest("Failed to create order.");
            }

            return Ok(response);
        }

    }
}
