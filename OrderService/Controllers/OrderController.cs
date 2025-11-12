using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using OrderService.ProductClientService;
using System.Linq;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly ProductService _productService;

        public OrderController(OrderDbContext context, ProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        // ✅ POST: api/order
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (order == null || order.OrderItems == null || order.OrderItems.Count == 0)
                return BadRequest("Order must contain at least one item.");

            foreach (var item in order.OrderItems) {
               
                if(item == null) continue;
                int productid = item.ProductId;

                var product=await _productService.GetProductByIdAsync(productid);

                if (product.StockQuantity < item.Quantity)
                {
                    return BadRequest("Product is Out of stock…!!!");
                }
                
                    int rem=product.StockQuantity-item.Quantity;
                    await _productService.UpdateProductStockAsync(productid,rem);
                

            }
            order.TotalAmount = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);
            order.OrderDate = DateTime.UtcNow;
            order.Status = "Pending";

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }

        // ✅ GET: api/order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ToListAsync();

            return Ok(orders);
        }

        // ✅ GET: api/order/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        // ✅ PUT: api/order/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            order.Status = status;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ DELETE: api/order/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("/GrandTotal/{id}")]


        public async Task<IActionResult> GetTotalAmount(int id)
        {
            var order = await _context.Orders
                        .Include(o => o.OrderItems)
                        .FirstOrDefaultAsync(o => o.OrderId == id);


            if (order == null) return NotFound("Order Not Found");
            decimal GrandTotal = 0;
            foreach(var item in order.OrderItems)
            {
                GrandTotal += item.Subtotal;
            }


            return Ok(GrandTotal);
        }
    }
}
