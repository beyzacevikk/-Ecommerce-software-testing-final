using System;
using ECommerceApp.Core;

namespace ECommerceApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("   E-Ticaret Sistemi - Demo Çalıştırma");
            Console.WriteLine("========================================\n");

            // Ürünler oluştur
            var laptop = new Product(1, "Laptop", 15000m, 10);
            var mouse = new Product(2, "Mouse", 250m, 50);
            var keyboard = new Product(3, "Klavye", 500m, 30);
            var monitor = new Product(4, "Monitör", 8000m, 5);

            Console.WriteLine("📦 Ürünler:");
            Console.WriteLine($"  - {laptop.Name}: {laptop.Price} TL (Stok: {laptop.Stock})");
            Console.WriteLine($"  - {mouse.Name}: {mouse.Price} TL (Stok: {mouse.Stock})");
            Console.WriteLine($"  - {keyboard.Name}: {keyboard.Price} TL (Stok: {keyboard.Stock})");
            Console.WriteLine($"  - {monitor.Name}: {monitor.Price} TL (Stok: {monitor.Stock})");

            // Sepet oluştur
            var cart = new Cart();
            cart.AddItem(laptop, 1);
            cart.AddItem(mouse, 2);

            Console.WriteLine($"\n🛒 Sepet Toplam: {cart.GetTotal()} TL");
            Console.WriteLine($"   Ürün Adedi: {cart.GetItemCount()}");

            // Sipariş ver
            var orderService = new OrderService();
            var order = orderService.PlaceOrder(cart, 0);

            Console.WriteLine($"\n📋 Sipariş #{order.OrderId}");
            Console.WriteLine($"   Durum: {order.Status}");
            Console.WriteLine($"   Toplam: {order.TotalAmount} TL");
            Console.WriteLine($"   Ödenecek: {order.FinalAmount} TL");

            if (order.Status == OrderStatus.Failed)
                Console.WriteLine($"   ❌ Hata: {order.FailReason}");

            // Ödeme
            if (order.Status == OrderStatus.Confirmed)
            {
                bool paid = orderService.ProcessPayment(order, order.FinalAmount);
                Console.WriteLine($"   💳 Ödeme: {(paid ? "Başarılı ✅" : "Başarısız ❌")}");
            }

            Console.WriteLine("\n📊 Güncel Stok:");
            Console.WriteLine($"  - {laptop.Name}: {laptop.Stock}");
            Console.WriteLine($"  - {mouse.Name}: {mouse.Stock}");

            Console.WriteLine("\n========================================");
            Console.WriteLine("   Demo tamamlandı.");
            Console.WriteLine("========================================");
        }
    }
}
