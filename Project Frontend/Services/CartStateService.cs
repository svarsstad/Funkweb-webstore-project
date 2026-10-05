// Services/CartStateService.cs
using Project_Backend.Models;
using Project_Frontend.Components.Pages;

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

        public void addQuantity(Product product, PublicProductService publicProductService, int quantity = 1)
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

        public async void UpdateQuantity(string productId, int value, PublicProductService publicProductService)
        {
            var item = Items.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null)
            {
                item.Quantity = value;
                if (item.Quantity <= 0)
                {
                    Items.Remove(item);
                }
                NotifyStateChanged();
            }
            else
            {
                Product? newProduct = await publicProductService.GetProductByIdAsync(productId);

                // 2. Safely check for null before adding
                if (newProduct != null)
                {
                    addQuantity(newProduct, publicProductService, value);
                }
                else
                {
                    // Handle case where product was not found (e.g., log error or alert user)
                }
            }
            if (item.Quantity <= 0)
            {
                RemoveItem(item.Product.Id);
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