// Services/CartStateService.cs
using Project_Backend.Models;

namespace Project_Frontend.Services
{
    public class CartStateService
    {
        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
        // Active cart items state
        public List<CartItem> Items = new();

        // Event raised whenever cart data changes

        public int TotalCount => Items.Sum(i => i.Quantity);
        public decimal SubTotal => Items.Sum(i => i.Product.Price * i.Quantity);

        public void AddProduct(Product product, int quantity = 1)
        {
            var existing = Items.FirstOrDefault(i => i.Product.Id == product.Id);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem { Product = product, Quantity = quantity });
            }
            NotifyStateChanged();
        }

        public void UpdateQuantity(string productId, int delta)
        {
            var item = Items.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null)
            {
                item.Quantity += delta;
                if (item.Quantity <= 0)
                {
                    Items.Remove(item);
                }
                NotifyStateChanged();
            }
        }

        public void RemoveItem(string productId)
        {
            Items.RemoveAll(i => i.Product.Id == productId);
            NotifyStateChanged();
        }
        public int FindItem(string productId)
        {
            var existing = Items.FirstOrDefault(i => i.Product.Id == productId);
            if (existing != null)
            {
               return existing.Quantity;
            }
            else
            {
                return 0;
            }
        }

        public void ClearCart()
        {
            Items.Clear();
            NotifyStateChanged();
        }

        
    }

    public class CartItem
    {
        public Product Product { get; set; } = new();
        public int Quantity { get; set; } = 1;
    }
}