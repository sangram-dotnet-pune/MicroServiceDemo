namespace OrderService.Models
{
   
        public class Product
        {


            public int ProductId { get; set; }
            public string Name { get; set; }             // Product name
            public string Description { get; set; }      // Short description
            public decimal Price { get; set; }           // Price per unit
            public int StockQuantity { get; set; }       // Available stock
            public string Category { get; set; }         // Category (e.g. Electronics)
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp
        }
    
}
