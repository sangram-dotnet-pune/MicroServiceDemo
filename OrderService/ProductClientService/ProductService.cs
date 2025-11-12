using OrderService.Models;

namespace OrderService.ProductClientService
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            // Example endpoint of Product Microservice
            return await _httpClient.GetFromJsonAsync<Product>($"api/product/{id}");
        }

        public async Task<bool> UpdateProductStockAsync(int productId, int newStock)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/product/{productId}/stock", new { StockQuantity = newStock });
            return response.IsSuccessStatusCode;
        }
    }
}
