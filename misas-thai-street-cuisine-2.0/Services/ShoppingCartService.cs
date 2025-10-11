using misas_thai_street_cuisine_2._0.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class ShoppingCartService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly MenuService _menuService;
        private const string LocalStorageKey = "misas-cart";
        private bool _isInitialized = false;
        private Task? _initializationTask;
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        
        public bool IsInitialized => _isInitialized;

        public ShoppingCartService(IJSRuntime jsRuntime, MenuService menuService)
        {
            _jsRuntime = jsRuntime;
            _menuService = menuService;
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized) return;
            
            // Ensure only one initialization happens even with concurrent calls
            if (_initializationTask != null)
            {
                await _initializationTask;
                return;
            }

            _initializationTask = InitializeInternalAsync();
            await _initializationTask;
        }

        private async Task InitializeInternalAsync()
        {
            try
            {
                await LoadFromLocalStorage();
                _isInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing cart from localStorage: {ex.Message}");
                // If loading fails, start with empty cart
                Items = new List<CartItem>();
                _isInitialized = true;
            }
        }

        private async Task LoadFromLocalStorage()
        {
            try
            {
                var cartJson = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", LocalStorageKey);
                
                if (!string.IsNullOrEmpty(cartJson))
                {
                    Console.WriteLine($"Loading cart JSON: {cartJson}");
                    
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        WriteIndented = false
                    };
                    
                    var savedDtos = JsonSerializer.Deserialize<List<CartItemDto>>(cartJson, options);
                    Items = ConvertDtosToCartItems(savedDtos ?? new List<CartItemDto>());
                    
                    Console.WriteLine($"Successfully loaded {Items.Count} items from localStorage");
                }
                else
                {
                    Console.WriteLine("No cart data found in localStorage");
                    Items = new List<CartItem>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading cart from localStorage: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Try to clear corrupted data
                try
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", LocalStorageKey);
                    Console.WriteLine("Cleared corrupted cart data from localStorage");
                }
                catch (Exception clearEx)
                {
                    Console.WriteLine($"Error clearing localStorage: {clearEx.Message}");
                }
                
                Items = new List<CartItem>();
            }
        }

        private async Task SaveToLocalStorage()
        {
            if (!_isInitialized) return;
            
            try
            {
                var dtos = ConvertCartItemsToDtos(Items);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = false
                };
                
                var cartJson = JsonSerializer.Serialize(dtos, options);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", LocalStorageKey, cartJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving cart to localStorage: {ex.Message}");
            }
        }

        public async Task AddItemAsync(CartItem cartItem)
        {
            await InitializeAsync(); // Ensure cart is initialized
            
            // Find existing item with same name, category, and serves
            var existingItemIndex = Items.FindIndex(i =>
                i.Item.Name == cartItem.Item.Name &&
                i.Item.Category == cartItem.Item.Category &&
                i.SelectedServes == cartItem.SelectedServes);

            if (existingItemIndex >= 0)
            {
                // Replace existing item at the same position to preserve order
                Items[existingItemIndex] = cartItem;
            }
            else
            {
                // Add new item to the end
                Items.Add(cartItem);
            }
            
            await SaveToLocalStorage();
            OnItemAddedOrRemoved();
        }

        private async Task EnsureInitializedAsync()
        {
            if (!_isInitialized)
            {
                await InitializeAsync();
            }
        }

        public void AddItem(CartItem cartItem)
        {
            // Try to initialize if not already done (fire and forget for sync method)
            if (!_isInitialized)
            {
                _ = Task.Run(async () => await InitializeAsync());
            }

            // Find existing item with same name, category, and serves
            var existingItemIndex = Items.FindIndex(i =>
                i.Item.Name == cartItem.Item.Name &&
                i.Item.Category == cartItem.Item.Category &&
                i.SelectedServes == cartItem.SelectedServes);

            if (existingItemIndex >= 0)
            {
                // Replace existing item at the same position to preserve order
                Items[existingItemIndex] = cartItem;
            }
            else
            {
                // Add new item to the end
                Items.Add(cartItem);
            }
            OnItemAddedOrRemoved();
        }

        public async Task RemoveItemAsync(CartItem cartItem)
        {
            await InitializeAsync(); // Ensure cart is initialized
            
            var existingItem = Items.FirstOrDefault(i =>
                i.Item.Name == cartItem.Item.Name &&
                i.Item.Category == cartItem.Item.Category &&
                i.SelectedServes == cartItem.SelectedServes
            );
            if (existingItem != null)
            {
                existingItem.Quantity -= 1;
                if (existingItem.Quantity <= 0)
                {
                    Items.Remove(existingItem);
                }
            }
            
            await SaveToLocalStorage();
            OnItemAddedOrRemoved();
        }

        public void RemoveItem(CartItem cartItem)
        {
            var existingItem = Items.FirstOrDefault(i =>
                i.Item.Name == cartItem.Item.Name &&
                i.Item.Category == cartItem.Item.Category &&
                i.SelectedServes == cartItem.SelectedServes
            );
            if (existingItem != null)
            {
                existingItem.Quantity -= 1;
                if (existingItem.Quantity <= 0)
                {
                    Items.Remove(existingItem);
                }
            }
            OnItemAddedOrRemoved();
        }

        public async Task RemoveAllAsync(CartItem cartItem)
        {
            await InitializeAsync(); // Ensure cart is initialized
            
            var existingItem = Items.FirstOrDefault(i =>
                i.Item.Name == cartItem.Item.Name &&
                i.Item.Category == cartItem.Item.Category &&
                i.SelectedServes == cartItem.SelectedServes &&
                i.UpgradePhadThai24Qty == cartItem.UpgradePhadThai24Qty &&
                i.UpgradePhadThai48Qty == cartItem.UpgradePhadThai48Qty
            );
            if (existingItem != null)
            {
                Items.Remove(existingItem);
            }
            
            await SaveToLocalStorage();
            OnItemAddedOrRemoved();
        }

        public void RemoveAll(CartItem cartItem)
        {
            var existingItem = Items.FirstOrDefault(i =>
                i.Item.Name == cartItem.Item.Name &&
                i.Item.Category == cartItem.Item.Category &&
                i.SelectedServes == cartItem.SelectedServes &&
                i.UpgradePhadThai24Qty == cartItem.UpgradePhadThai24Qty &&
                i.UpgradePhadThai48Qty == cartItem.UpgradePhadThai48Qty
            );
            if (existingItem != null)
            {
                Items.Remove(existingItem);
            }
            OnItemAddedOrRemoved();
        }

        public event Action CartChanged = delegate { };

        private void OnItemAddedOrRemoved()
        {
            CartChanged?.Invoke();
        }


        public decimal GetTotalPrice()
        {
            return Items.Sum(i => i.GetTotalPrice());
        }

        public int GetItemQuantity(CartItem cartItem)
        {
            var existingItem = Items.FirstOrDefault(i =>
                i.Item.Name == cartItem.Item.Name &&
                i.Item.Category == cartItem.Item.Category &&
                i.SelectedServes == cartItem.SelectedServes &&
                i.UpgradePhadThai24Qty == cartItem.UpgradePhadThai24Qty &&
                i.UpgradePhadThai48Qty == cartItem.UpgradePhadThai48Qty
            );
            return existingItem?.Quantity ?? 0;
        }

        public int GetQuantity(CartItem cartItem)
        {
            return GetItemQuantity(cartItem);
        }

        public async Task ClearCartAsync()
        {
            await InitializeAsync(); // Ensure cart is initialized
            
            Items.Clear();
            await SaveToLocalStorage();
            OnItemAddedOrRemoved();
        }

        public void ClearCart()
        {
            Items.Clear();
            OnItemAddedOrRemoved();
        }

        private List<CartItemDto> ConvertCartItemsToDtos(List<CartItem> cartItems)
        {
            return cartItems.Select(item => new CartItemDto
            {
                ItemName = item.Item.Name,
                Category = item.Item.Category,
                Quantity = item.Quantity,
                SelectedServes = item.SelectedServes,
                UpgradePhadThai24Qty = item.UpgradePhadThai24Qty,
                UpgradePhadThai48Qty = item.UpgradePhadThai48Qty
            }).ToList();
        }

        private List<CartItem> ConvertDtosToCartItems(List<CartItemDto> dtos)
        {
            var cartItems = new List<CartItem>();
            
            foreach (var dto in dtos)
            {
                var menuItem = _menuService.GetMenuItem(dto.ItemName, dto.Category);
                if (menuItem != null)
                {
                    cartItems.Add(new CartItem(menuItem, dto.Quantity, dto.SelectedServes, dto.UpgradePhadThai24Qty, dto.UpgradePhadThai48Qty));
                }
                else
                {
                    Console.WriteLine($"Warning: Could not find menu item '{dto.ItemName}' in category '{dto.Category}'");
                }
            }
            
            return cartItems;
        }

    }
}
