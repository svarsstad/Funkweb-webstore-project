namespace Project_Backend.Models
{
    public class PayRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<OrderItem> cart { get; set; } = new List<OrderItem>();
    }
    public class PayResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
