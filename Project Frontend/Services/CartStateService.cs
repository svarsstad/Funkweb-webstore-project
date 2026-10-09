// Services/CartStateService.cs
using Project_Backend.Models;
using Project_Frontend.Components.Pages;

namespace Project_Frontend.Services
{
    public class CartStateService
    {
        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
        Project_Frontend.Components.Layout.MainLayout? mainLayout = null;
        public void SetMainLayout(Project_Frontend.Components.Layout.MainLayout ML)
        {
            mainLayout = ML;
        }
        // Active cart items state
        public List<CartItem> Items = new();

        // Event raised whenever cart data changes

        public int TotalCount => Items.Sum(i => i.Quantity);
        public decimal SubTotal => Items.Sum(i => i.Product.Price * i.Quantity);

        public Task addQuantity(Product product, PublicProductService publicProductService, int quantity = 1)
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

            int tot = Items.Sum(c => c.Quantity);
            if (mainLayout != null)
            {
                mainLayout.UpdateCartCount(tot);
            }

            // Do not call component instance methods from a service.
            // Notify subscribers; components should call StateHasChanged / InvokeAsync themselves.
            NotifyStateChanged();

            return Task.CompletedTask;
        }

        public async Task UpdateQuantity(string productId, int value, PublicProductService publicProductService)
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

                if (newProduct != null)
                {
                    await addQuantity(newProduct, publicProductService, value);
                }
                else
                {
                    // Product not found — handle as needed
                }
            }

            // Only operate on item if it's not null
            if (item != null && item.Quantity <= 0)
            {
                RemoveItem(item.Product.Id);
            }
            if(mainLayout != null)
            {
                mainLayout.UpdateCartCount(Items.Sum(c => c.Quantity));
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