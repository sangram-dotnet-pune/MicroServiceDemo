namespace OrderService.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }       // Primary key
        public int OrderId { get; set; }           // FK to Order
        public int ProductId { get; set; }         // FK (references ProductService)
        public int Quantity { get; set; }          // How many units
        public decimal UnitPrice { get; set; }     // Price at time of order
        public decimal Subtotal => Quantity * UnitPrice;  // Derived field
    }
}
