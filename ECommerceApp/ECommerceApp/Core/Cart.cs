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

        /// <summary>
        /// Kargo ücreti hesaplama.
        /// 200 TL ve üzeri siparişlerde kargo ücretsiz.
        /// BUG #7: Sınır değerde (200 TL) kargo ücreti alınıyor.
        ///         >= yerine > kullanılmış.
        /// </summary>
        public decimal CalculateShippingFee()
        {
            decimal total = GetTotal();
            decimal shippingFee = 29.99m;

            // BUG: total > 200 kullanılmış, total >= 200 olmalı
            // 200 TL'de hâlâ kargo ücreti alınıyor
            if (total > 200m)
                return 0m;

            return shippingFee;
        }
    }

    /// <summary>
    /// Kupon kodu sistemi.
    /// </summary>
    public class Coupon
    {
        public string Code { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
        public decimal MinOrderAmount { get; set; }

        public Coupon(string code, decimal discountPercent, DateTime expiryDate, decimal minOrderAmount = 0)
        {
            Code = code;
            DiscountPercent = discountPercent;
            ExpiryDate = expiryDate;
            IsUsed = false;
            MinOrderAmount = minOrderAmount;
        }

        /// <summary>
        /// Kupon geçerli mi kontrolü.
        /// BUG #8: Süresi dolmuş kupon kontrolünde tarih karşılaştırması yanlış.
        ///         Bugünün tarihi son gün ise kupon geçersiz sayılıyor.
        /// </summary>
        public bool IsValid(decimal orderTotal)
        {
            if (IsUsed)
                return false;

            if (string.IsNullOrEmpty(Code))
                return false;

            // BUG: ExpiryDate > DateTime.Now yerine >= olmalı
            // Son gün kupon kullanılamıyor
            if (ExpiryDate < DateTime.Now)
                return false;

            // BUG #8: MinOrderAmount kontrolü yok!
            // Doğrusu: if (orderTotal < MinOrderAmount) return false;
            // Minimum tutar altında da kupon geçerli sayılıyor

            return true;
        }

        /// <summary>
        /// Kuponu kullanıldı olarak işaretle.
        /// </summary>
        public void MarkAsUsed()
        {
            IsUsed = true;
        }
    }
}
