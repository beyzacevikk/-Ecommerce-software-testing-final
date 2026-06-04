namespace ECommerceApp.Core
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public Product(int id, string name, decimal price, int stock)
        {
            Id = id;
            Name = name;
            Price = price;
            Stock = stock;
        }

        /// <summary>
        /// Stoktan düşürme işlemi.
        /// BUG #1: Stok 0 olduğunda hâlâ düşürmeye izin veriyor (>= yerine > kullanılmış).
        /// Doğru kontrol: if (quantity > 0 && quantity <= Stock)
        /// </summary>
        public bool ReduceStock(int quantity)
        {
            // HATA: quantity >= 0 olması gerekirken quantity > 0 olmalı
            // ama asıl sorun: 0 stokta bile true dönüyor çünkü
            // kontrol "Stock > quantity" yerine "Stock >= quantity" olmalı
            if (quantity > 0 && Stock > quantity)
            {
                Stock -= quantity;
                return true;
            }
            // BUG: Stock == quantity durumunda false dönüyor
            // Örn: Stock=5, quantity=5 → false döner (olmamalı)
            return false;
        }

        /// <summary>
        /// Ürün geçerli mi kontrolü.
        /// BUG #2: Negatif fiyat kontrolü yok.
        /// </summary>
        public bool IsValid()
        {
            // BUG: Price > 0 kontrolü eksik, Price = 0 veya negatif geçebilir
            return !string.IsNullOrEmpty(Name) && Stock >= 0;
        }
    }
}
