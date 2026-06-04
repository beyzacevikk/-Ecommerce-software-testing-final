using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceApp.Core
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public decimal GetSubtotal()
        {
            return Product.Price * Quantity;
        }
    }

    public class Cart
    {
        public List<CartItem> Items { get; private set; }

        public Cart()
        {
            Items = new List<CartItem>();
        }

        /// <summary>
        /// Sepete ürün ekleme.
        /// Aynı üründen varsa miktarı artırır.
        /// </summary>
        public bool AddItem(Product product, int quantity)
        {
            if (product == null || quantity <= 0)
                return false;

            var existing = Items.FirstOrDefault(i => i.Product.Id == product.Id);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem(product, quantity));
            }
            return true;
        }

        /// <summary>
        /// Sepetten ürün çıkarma.
        /// </summary>
        public bool RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null)
            {
                Items.Remove(item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sepetteki toplam tutarı hesaplar.
        /// </summary>
        public decimal GetTotal()
        {
            return Items.Sum(i => i.GetSubtotal());
        }

        /// <summary>
        /// İndirim uygular.
        /// BUG #3: %50'den büyük indirim kontrolü yok.
        /// BUG #4: İndirim hesaplaması hatalı — yüzde yerine oran olarak
        ///         kullanılması gerekirken 100'e bölme unutulmuş.
        ///         Örn: %10 indirim için discountPercent=10 gönderildiğinde
        ///         total * 10 yerine total * 0.10 olmalı.
        /// </summary>
        public decimal ApplyDiscount(decimal discountPercent)
        {
            if (discountPercent < 0)
                return GetTotal();

            // BUG: discountPercent > 50 kontrolü yok, %100 bile uygulanabilir
            // BUG: Hesaplama hatası — discountPercent 100'e bölünmemiş
            // Doğrusu: decimal discountAmount = total * (discountPercent / 100m);
            decimal total = GetTotal();
            decimal discountAmount = total * discountPercent; // BUG: 100'e bölme yok!
            return total - discountAmount;
        }

        /// <summary>
        /// Sepet boş mu kontrolü.
        /// </summary>
        public bool IsEmpty()
        {
            return Items.Count == 0;
        }

        /// <summary>
        /// Sepetteki toplam ürün adedi.
        /// </summary>
        public int GetItemCount()
        {
            return Items.Sum(i => i.Quantity);
        }

        /// <summary>
        /// Sepeti temizler.
        /// </summary>
        public void Clear()
        {
            Items.Clear();
        }
    }
}
