using Api.Data;
using Api.Data.Entity;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RabbitMQService _rabbitMQService;
        private readonly OrderPublisher _orderPublisher;
        public OrdersController(ApplicationDbContext context, RabbitMQService rabbitMQService, OrderPublisher orderPublisher)
        {
            _context = context;
            _rabbitMQService = rabbitMQService;
            _orderPublisher = orderPublisher;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return Ok(orders);
        }

        // POST: api/orders
        [HttpPost]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order cannot be null.");
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            // Mesajı RabbitMQ'ya gönder
            _rabbitMQService.SendMessage($"Order Created: {order.Id}");
            // Sipariş oluşturulduğunda mesajı RabbitMQ'ya gönder
            _orderPublisher.PublishOrderCreated(order);

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
    }

}
