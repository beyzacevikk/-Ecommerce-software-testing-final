using System;
using System.Collections.Generic;

namespace ECommerceApp.Core
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Failed,
        Cancelled
    }

    public class Order
    {
        public int OrderId { get; set; }
        public List<CartItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal FinalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public string FailReason { get; set; }

        public Order()
        {
            Items = new List<CartItem>();
            OrderDate = DateTime.Now;
            Status = OrderStatus.Pending;
            FailReason = string.Empty;
        }
    }

    public class OrderService
    {
        // Minimum sipariş tutarı sabiti
        public const decimal MinimumOrderAmount = 50.0m;
        // Maksimum indirim yüzdesi
        public const decimal MaxDiscountPercent = 50.0m;

        private int _nextOrderId = 1;
        private List<Order> _orders;

        public OrderService()
        {
            _orders = new List<Order>();
        }

        /// <summary>
        /// Sipariş verme işlemi.
        /// Stok kontrolü, minimum tutar kontrolü ve indirim uygular.
        /// BUG #5: Minimum sipariş kontrolünde < yerine <= kullanılmış.
        ///         50 TL tam sınırda olan sipariş reddediliyor.
        /// BUG #6: Stok kontrolü quantity > stock yerine quantity >= stock
        ///         olduğu için stok tam yettiğinde reddediyor (Product.ReduceStock bug'ından kaynaklı).
        /// </summary>
        public Order PlaceOrder(Cart cart, decimal discountPercent = 0)
        {
            var order = new Order();
            order.OrderId = _nextOrderId++;

            // Sepet boş mu?
            if (cart == null || cart.IsEmpty())
            {
                order.Status = OrderStatus.Failed;
                order.FailReason = "Sepet boş.";
                _orders.Add(order);
                return order;
            }

            // Stok kontrolü
            foreach (var item in cart.Items)
            {
                if (item.Product.Stock < item.Quantity)
                {
                    order.Status = OrderStatus.Failed;
                    order.FailReason = $"Yetersiz stok: {item.Product.Name} (Stok: {item.Product.Stock}, İstenen: {item.Quantity})";
                    _orders.Add(order);
                    return order;
                }
            }

            // Toplam tutar
            decimal total = cart.GetTotal();
            order.TotalAmount = total;

            // BUG #5: Minimum sipariş kontrolü — <= yerine < olmalı
            // Doğrusu: if (total < MinimumOrderAmount)
            if (total <= MinimumOrderAmount)
            {
                order.Status = OrderStatus.Failed;
                order.FailReason = $"Minimum sipariş tutarı {MinimumOrderAmount} TL. Mevcut: {total} TL";
                _orders.Add(order);
                return order;
            }

            // İndirim uygulama
            order.DiscountPercent = discountPercent;
            if (discountPercent > 0)
            {
                // BUG: MaxDiscount kontrolü yapılmıyor!
                // Aşağıdaki satır olması gerekirdi ama yok:
                // if (discountPercent > MaxDiscountPercent) { ... fail ... }
                order.FinalAmount = cart.ApplyDiscount(discountPercent);
            }
            else
            {
                order.FinalAmount = total;
            }

            // Stok düşürme
            foreach (var item in cart.Items)
            {
                // BUG #6: Product.ReduceStock'taki bug yüzünden
                // Stock == Quantity durumunda stok düşürülemiyor
                bool reduced = item.Product.ReduceStock(item.Quantity);
                if (!reduced)
                {
                    order.Status = OrderStatus.Failed;
                    order.FailReason = $"Stok düşürme başarısız: {item.Product.Name}";
                    _orders.Add(order);
                    return order;
                }
            }

            // Sipariş öğelerini kaydet
            order.Items = new List<CartItem>(cart.Items);
            order.Status = OrderStatus.Confirmed;
            _orders.Add(order);

            return order;
        }

        /// <summary>
        /// Siparişi iptal etme.
        /// </summary>
        public bool CancelOrder(int orderId)
        {
            var order = _orders.Find(o => o.OrderId == orderId);
            if (order != null && order.Status == OrderStatus.Confirmed)
            {
                order.Status = OrderStatus.Cancelled;
                // Stokları geri yükle
                foreach (var item in order.Items)
                {
                    item.Product.Stock += item.Quantity;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sipariş sorgulama.
        /// </summary>
        public Order GetOrder(int orderId)
        {
            return _orders.Find(o => o.OrderId == orderId);
        }

        /// <summary>
        /// Tüm siparişleri getir.
        /// </summary>
        public List<Order> GetAllOrders()
        {
            return new List<Order>(_orders);
        }

        /// <summary>
        /// Ödeme simülasyonu.
        /// </summary>
        public bool ProcessPayment(Order order, decimal paymentAmount)
        {
            if (order == null || order.Status != OrderStatus.Confirmed)
                return false;

            return paymentAmount >= order.FinalAmount;
        }
    }
}
