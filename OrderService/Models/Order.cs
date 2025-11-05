namespace OrderService.Models
{
    public class Order
    {
        public int OrderId { get; set; }                  // Primary key
        public string CustomerName { get; set; }          // Customer name
        public string CustomerEmail { get; set; }         // Customer email/contact
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;  // Timestamp
        public decimal TotalAmount { get; set; }          // Calculated total
        public string Status { get; set; } = "Pending";   // e.g., Pending, Shipped, Delivered

        public List<OrderItem> OrderItems { get; set; }
    }
}
