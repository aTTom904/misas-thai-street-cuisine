using misas_thai_street_cuisine_2._0.Models;
using Microsoft.JSInterop;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class ShoppingCartService
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();


        public void AddItem(CartItem cartItem)
        {
            // Uniqueness: match on item, selected serves, and upgrades
            var existingItem = Items.FirstOrDefault(i =>
                i.Item.Name == cartItem.Item.Name &&
                i.Item.Category == cartItem.Item.Category &&
                i.SelectedServes == cartItem.SelectedServes &&
                i.UpgradePhadThai24Qty == cartItem.UpgradePhadThai24Qty &&
                i.UpgradePhadThai48Qty == cartItem.UpgradePhadThai48Qty
            );
            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity;
            }
            else
            {
                Items.Add(cartItem);
            }
            OnItemAddedOrRemoved();
        }



        public void RemoveItem(CartItem cartItem)
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
                existingItem.Quantity -= 1;
                if (existingItem.Quantity <= 0)
                {
                    Items.Remove(existingItem);
                }
            }
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

        public void ClearCart()
        {
            Items.Clear();
            OnItemAddedOrRemoved();
        }

    }
}
