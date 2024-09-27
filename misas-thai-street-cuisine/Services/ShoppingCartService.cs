using Models;
using Microsoft.JSInterop;

namespace Services
{
    public class ShoppingCartService
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public void AddItem(ICartItem item, int quantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.Item.Name == item.Name);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem(item, quantity));
            }
        }

        public void RemoveItem(ICartItem item)
        {
            var cartItem = Items.FirstOrDefault(i => i.Item.Name == item.Name);
            if (cartItem != null)
            {
                Items.Remove(cartItem);
            }
        }

        public decimal GetTotalPrice()
        {
            return Items.Sum(i => i.GetTotalPrice());
        }

    }
}
