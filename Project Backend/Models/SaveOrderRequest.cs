namespace Project_Backend.Models
{
    public class SaveOrderRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string OrderDate { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;
        public OrderItem[] Items { get; set; } = new OrderItem[0];
        public double TotalOrderValue { get; set; } = 0.0;

        public SaveOrderRequest(string? userID, OrderItem[] oItems, double[] price)
        {
            if (string.IsNullOrWhiteSpace(userID)) { UserId = ""; }
            else {
                UserId = userID;
            }
            OrderDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
            OrderStatus = "Pending Payment";
            Items = oItems;
            TotalOrderValue = price.Sum();
        }
    }
}
