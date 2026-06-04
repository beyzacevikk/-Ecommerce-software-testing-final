using NUnit.Framework;
using ECommerceApp.Core;

namespace ECommerceApp.Tests.IntegrationTests
{
    /// <summary>
    /// INTEGRATION TESTS
    /// Birden fazla bileşenin birlikte çalışmasını test eder.
    /// Product + Cart + OrderService entegrasyonu.
    /// </summary>
    [TestFixture]
    public class OrderIntegrationTests
    {
        private OrderService _orderService;

        [SetUp]
        public void Setup()
        {
            _orderService = new OrderService();
        }

        // =====================================================
        // TEST 24 - Integration: Tam sipariş akışı (ürün seç → sepete ekle → sipariş ver → öde)
        // =====================================================
        [Test]
        public void FullOrderFlow_ValidScenario_ShouldCompleteSuccessfully()
        {
            // 1. Ürün oluştur
            var laptop = new Product(1, "Laptop", 15000m, 10);
            var mouse = new Product(2, "Mouse", 250m, 50);

            // 2. Sepete ekle
            var cart = new Cart();
            Assert.That(cart.AddItem(laptop, 1), Is.True);
            Assert.That(cart.AddItem(mouse, 2), Is.True);
            Assert.That(cart.GetTotal(), Is.EqualTo(15500m));

            // 3. Sipariş ver
            var order = _orderService.PlaceOrder(cart);
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Confirmed));
            Assert.That(order.FinalAmount, Is.EqualTo(15500m));

            // 4. Ödeme yap
            bool paid = _orderService.ProcessPayment(order, 15500m);
            Assert.That(paid, Is.True, "Ödeme başarılı olmalı.");

            // 5. Stok kontrolü
            Assert.That(laptop.Stock, Is.EqualTo(9), "Laptop stoku 10-1=9 olmalı.");
            Assert.That(mouse.Stock, Is.EqualTo(48), "Mouse stoku 50-2=48 olmalı.");
        }

        // =====================================================
        // TEST 25 - Integration: Sipariş iptal → stok geri yükleme
        // =====================================================
        [Test]
        public void CancelOrder_ShouldRestoreStock()
        {
            var product = new Product(1, "Telefon", 10000m, 20);
            var cart = new Cart();
            cart.AddItem(product, 3);

            var order = _orderService.PlaceOrder(cart);
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Confirmed));
            Assert.That(product.Stock, Is.EqualTo(17));

            // İptal et
            bool cancelled = _orderService.CancelOrder(order.OrderId);
            Assert.That(cancelled, Is.True);
            Assert.That(product.Stock, Is.EqualTo(20), "Stok geri yüklenmeli.");
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Cancelled));
        }

        // =====================================================
        // TEST 26 - Integration: Yetersiz ödeme
        // =====================================================
        [Test]
        public void ProcessPayment_InsufficientAmount_ShouldFail()
        {
            var product = new Product(1, "Kulaklık", 500m, 10);
            var cart = new Cart();
            cart.AddItem(product, 2); // 1000 TL

            var order = _orderService.PlaceOrder(cart);
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Confirmed));

            bool paid = _orderService.ProcessPayment(order, 999m); // 999 < 1000
            Assert.That(paid, Is.False, "Yetersiz ödeme reddedilmeli.");
        }

        // =====================================================
        // TEST 27 - Integration: Birden fazla sipariş → stok tutarlılığı
        // =====================================================
        [Test]
        public void MultipleOrders_StockShouldBeConsistent()
        {
            var product = new Product(1, "Tablet", 3000m, 10);

            // İlk sipariş: 3 adet
            var cart1 = new Cart();
            cart1.AddItem(product, 3);
            var order1 = _orderService.PlaceOrder(cart1);
            Assert.That(order1.Status, Is.EqualTo(OrderStatus.Confirmed));
            Assert.That(product.Stock, Is.EqualTo(7));

            // İkinci sipariş: 4 adet
            var cart2 = new Cart();
            cart2.AddItem(product, 4);
            var order2 = _orderService.PlaceOrder(cart2);
            Assert.That(order2.Status, Is.EqualTo(OrderStatus.Confirmed));
            Assert.That(product.Stock, Is.EqualTo(3));

            // Üçüncü sipariş: 5 adet (stok yetmemeli)
            var cart3 = new Cart();
            cart3.AddItem(product, 5);
            var order3 = _orderService.PlaceOrder(cart3);
            Assert.That(order3.Status, Is.EqualTo(OrderStatus.Failed));
            Assert.That(product.Stock, Is.EqualTo(3), "Başarısız sipariş stoku değiştirmemeli.");
        }
    }
}
