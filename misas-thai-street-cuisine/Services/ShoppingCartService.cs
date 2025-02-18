﻿using Models;
using Microsoft.JSInterop;

namespace Services
{
    public class ShoppingCartService
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public void AddItem(ICartItem item, int quantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.Item.Name == item.Name && i.Item.Category == item.Category);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem(item, quantity));
            }
            OnItemAddedOrRemoved();
        }

        public void RemoveItem(ICartItem item)
        {
            var cartItem = Items.FirstOrDefault(i => i.Item.Name == item.Name);
            if (cartItem != null)
            {
                cartItem.Quantity -= 1;

                if (cartItem.Quantity == 0)
                {
                    Items.Remove(cartItem);
                }
            }

            OnItemAddedOrRemoved();
        }

        public void RemoveAll(ICartItem item)
        {
            var cartItem = Items.FirstOrDefault(i => i.Item.Name == item.Name);
            if (cartItem != null)
            {
                Items.Remove(cartItem);
            }

            OnItemAddedOrRemoved();
        }

        public event Action CartChanged;

        private void OnItemAddedOrRemoved()
        {
            CartChanged?.Invoke();
        }


        public decimal GetTotalPrice()
        {
            return Items.Sum(i => i.GetTotalPrice());
        }

        public int GetItemQuantity(ICartItem item)
        {
            return Items.FirstOrDefault(ci => ci.Item == item)?.Quantity ?? 0;
        }

    }
}
