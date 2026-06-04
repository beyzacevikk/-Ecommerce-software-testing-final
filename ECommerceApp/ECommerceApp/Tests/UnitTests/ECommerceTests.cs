using NUnit.Framework;
using ECommerceApp.Core;

namespace ECommerceApp.Tests.UnitTests
{
    /// <summary>
    /// WHITE BOX UNIT TESTS
    /// İç yapıyı (kodu) bilerek yazılmış testler.
    /// Equivalence Partitioning (EP) ve Boundary Value Analysis (BVA) teknikleri kullanılmıştır.
    /// </summary>
    [TestFixture]
    public class ProductTests
    {
        // =====================================================
        // TEST 1 - Unit/WhiteBox - EP: Geçerli stok düşürme
        // Partition: quantity (1..Stock-1) → Geçerli
        // =====================================================
        [Test]
        public void ReduceStock_ValidQuantity_ShouldReduceAndReturnTrue()
        {
            // Arrange
            var product = new Product(1, "Laptop", 15000m, 10);

            // Act
            bool result = product.ReduceStock(3);

            // Assert
            Assert.That(result, Is.True, "Geçerli miktarda stok düşürme başarılı olmalı.");
            Assert.That(product.Stock, Is.EqualTo(7), "Stok 10 - 3 = 7 olmalı.");
        }

        // =====================================================
        // TEST 2 - Unit/WhiteBox - BVA: Stok sınır değer (Stock == Quantity)
        // BUG #1 TESPİTİ: Stock tam quantity'ye eşitken false dönüyor.
        // ReduceStock'ta "Stock > quantity" yerine "Stock >= quantity" olmalı.
        // =====================================================
        [Test]
        public void ReduceStock_QuantityEqualsStock_ShouldSucceed_BUG()
        {
            // Arrange
            var product = new Product(1, "Mouse", 250m, 5);

            // Act
            bool result = product.ReduceStock(5); // Stok = 5, miktar = 5

            // Assert
            // BUG: Bu test FAIL olacak çünkü Stock > quantity (5 > 5 = false)
            Assert.That(result, Is.True,
                "BUG #1: Stok miktarı talep edilen miktara tam eşitken sipariş kabul edilmeli. " +
                "ReduceStock metodunda '>' yerine '>=' kullanılmalı.");
        }

        // =====================================================
        // TEST 3 - Unit/WhiteBox - EP: Geçersiz miktar (0)
        // Partition: quantity <= 0 → Geçersiz
        // =====================================================
        [Test]
        public void ReduceStock_ZeroQuantity_ShouldReturnFalse()
        {
            var product = new Product(1, "Klavye", 500m, 10);

            bool result = product.ReduceStock(0);

            Assert.That(result, Is.False, "0 miktar için stok düşürme başarısız olmalı.");
            Assert.That(product.Stock, Is.EqualTo(10), "Stok değişmemeli.");
        }

        // =====================================================
        // TEST 4 - Unit/WhiteBox - EP: Negatif miktar
        // Partition: quantity < 0 → Geçersiz
        // =====================================================
        [Test]
        public void ReduceStock_NegativeQuantity_ShouldReturnFalse()
        {
            var product = new Product(1, "Monitör", 8000m, 10);

            bool result = product.ReduceStock(-1);

            Assert.That(result, Is.False, "Negatif miktar için stok düşürme başarısız olmalı.");
        }

        // =====================================================
        // TEST 5 - Unit/WhiteBox - BVA: Stoktan fazla miktar
        // Boundary: quantity = Stock + 1
        // =====================================================
        [Test]
        public void ReduceStock_QuantityExceedsStock_ShouldReturnFalse()
        {
            var product = new Product(1, "Tablet", 5000m, 3);

            bool result = product.ReduceStock(4); // Stok 3, talep 4

            Assert.That(result, Is.False, "Stoktan fazla miktar düşürülememeli.");
            Assert.That(product.Stock, Is.EqualTo(3), "Stok değişmemeli.");
        }

        // =====================================================
        // TEST 6 - Unit/WhiteBox - BUG #2 TESPİTİ: IsValid negatif fiyat
        // Product.IsValid() negatif fiyatı geçerli sayıyor.
        // =====================================================
        [Test]
        public void IsValid_NegativePrice_ShouldReturnFalse_BUG()
        {
            var product = new Product(1, "Hatalı Ürün", -100m, 10);

            bool result = product.IsValid();

            // BUG: Bu test FAIL olacak çünkü Price kontrolü yok
            Assert.That(result, Is.False,
                "BUG #2: Negatif fiyatlı ürün geçersiz olmalı. " +
                "IsValid metodunda Price > 0 kontrolü eksik.");
        }

        // =====================================================
        // TEST 7 - Unit/WhiteBox - EP: Geçerli ürün kontrolü
        // =====================================================
        [Test]
        public void IsValid_ValidProduct_ShouldReturnTrue()
        {
            var product = new Product(1, "Laptop", 15000m, 10);

            Assert.That(product.IsValid(), Is.True, "Geçerli ürün true dönmeli.");
        }
    }

    /// <summary>
    /// BLACK BOX TESTS
    /// İç yapı bilinmeden, sadece giriş/çıkış davranışına göre yazılmış testler.
    /// </summary>
    [TestFixture]
    public class CartBlackBoxTests
    {
        // =====================================================
        // TEST 8 - BlackBox - EP: Geçerli ürün ekleme
        // =====================================================
        [Test]
        public void AddItem_ValidProduct_ShouldAddToCart()
        {
            var cart = new Cart();
            var product = new Product(1, "Laptop", 15000m, 10);

            bool result = cart.AddItem(product, 2);

            Assert.That(result, Is.True);
            Assert.That(cart.GetItemCount(), Is.EqualTo(2));
            Assert.That(cart.GetTotal(), Is.EqualTo(30000m));
        }

        // =====================================================
        // TEST 9 - BlackBox - EP: Null ürün ekleme
        // =====================================================
        [Test]
        public void AddItem_NullProduct_ShouldReturnFalse()
        {
            var cart = new Cart();

            bool result = cart.AddItem(null, 1);

            Assert.That(result, Is.False);
            Assert.That(cart.IsEmpty(), Is.True);
        }

        // =====================================================
        // TEST 10 - BlackBox - BVA: Sıfır adet ekleme
        // =====================================================
        [Test]
        public void AddItem_ZeroQuantity_ShouldReturnFalse()
        {
            var cart = new Cart();
            var product = new Product(1, "Mouse", 250m, 50);

            bool result = cart.AddItem(product, 0);

            Assert.That(result, Is.False);
            Assert.That(cart.IsEmpty(), Is.True);
        }

        // =====================================================
        // TEST 11 - BlackBox - EP: Aynı ürünü tekrar ekleme (miktar artmalı)
        // =====================================================
        [Test]
        public void AddItem_SameProductTwice_ShouldMergeQuantities()
        {
            var cart = new Cart();
            var product = new Product(1, "Klavye", 500m, 30);

            cart.AddItem(product, 2);
            cart.AddItem(product, 3);

            Assert.That(cart.GetItemCount(), Is.EqualTo(5));
            Assert.That(cart.Items.Count, Is.EqualTo(1), "Aynı ürün tek satır olmalı.");
        }

        // =====================================================
        // TEST 12 - BlackBox - EP: Ürün çıkarma
        // =====================================================
        [Test]
        public void RemoveItem_ExistingProduct_ShouldRemove()
        {
            var cart = new Cart();
            var product = new Product(1, "Laptop", 15000m, 10);
            cart.AddItem(product, 1);

            bool result = cart.RemoveItem(1);

            Assert.That(result, Is.True);
            Assert.That(cart.IsEmpty(), Is.True);
        }

        // =====================================================
        // TEST 13 - BlackBox - EP: Olmayan ürünü çıkarma
        // =====================================================
        [Test]
        public void RemoveItem_NonExistingProduct_ShouldReturnFalse()
        {
            var cart = new Cart();

            bool result = cart.RemoveItem(999);

            Assert.That(result, Is.False);
        }

        // =====================================================
        // TEST 14 - BlackBox - BUG #3 & #4 TESPİTİ: İndirim hesaplaması
        // %10 indirim uygulandığında 100 TL'lik üründen 90 TL kalmalı.
        // BUG: 100'e bölme yapılmadığı için 100 * 10 = 1000 çıkartılıyor → -900 TL!
        // =====================================================
        [Test]
        public void ApplyDiscount_TenPercent_ShouldReturnCorrectAmount_BUG()
        {
            var cart = new Cart();
            var product = new Product(1, "Test Ürün", 100m, 10);
            cart.AddItem(product, 1); // Toplam: 100 TL

            decimal result = cart.ApplyDiscount(10); // %10 indirim

            // BUG: result = 100 - (100 * 10) = 100 - 1000 = -900 olacak!
            // Doğrusu: 100 - (100 * 10/100) = 100 - 10 = 90
            Assert.That(result, Is.EqualTo(90m),
                "BUG #4: İndirim hesaplaması hatalı. discountPercent 100'e bölünmemiş. " +
                "%10 indirimde 100 TL → 90 TL olmalı ama -900 TL dönüyor.");
        }

        // =====================================================
        // TEST 15 - BlackBox - BVA: %0 indirim (alt sınır)
        // =====================================================
        [Test]
        public void ApplyDiscount_ZeroPercent_ShouldReturnFullTotal()
        {
            var cart = new Cart();
            var product = new Product(1, "Ürün", 200m, 10);
            cart.AddItem(product, 1);

            decimal result = cart.ApplyDiscount(0);

            Assert.That(result, Is.EqualTo(200m), "%0 indirimde toplam değişmemeli.");
        }

        // =====================================================
        // TEST 16 - BlackBox - EP: Negatif indirim (geçersiz)
        // =====================================================
        [Test]
        public void ApplyDiscount_NegativePercent_ShouldReturnFullTotal()
        {
            var cart = new Cart();
            var product = new Product(1, "Ürün", 200m, 10);
            cart.AddItem(product, 1);

            decimal result = cart.ApplyDiscount(-5);

            Assert.That(result, Is.EqualTo(200m), "Negatif indirim uygulanmamalı.");
        }
    }

    /// <summary>
    /// GRAY BOX TESTS
    /// Kısmi iç bilgi ile yazılmış testler.
    /// Hem davranışı hem de bazı iç mekanizmaları test eder.
    /// </summary>
    [TestFixture]
    public class OrderServiceGrayBoxTests
    {
        // =====================================================
        // TEST 17 - GrayBox - EP: Boş sepet ile sipariş
        // İç bilgi: OrderService boş sepette "Sepet boş." mesajı döner
        // =====================================================
        [Test]
        public void PlaceOrder_EmptyCart_ShouldFail()
        {
            var service = new OrderService();
            var cart = new Cart();

            var order = service.PlaceOrder(cart);

            Assert.That(order.Status, Is.EqualTo(OrderStatus.Failed));
            Assert.That(order.FailReason, Does.Contain("Sepet boş"));
        }

        // =====================================================
        // TEST 18 - GrayBox - BUG #5 TESPİTİ: Minimum sipariş tutarı sınır değer
        // BVA: total == MinimumOrderAmount (50 TL)
        // BUG: <= kullanıldığı için 50 TL reddediliyor, < olmalı
        // =====================================================
        [Test]
        public void PlaceOrder_ExactMinimumAmount_ShouldSucceed_BUG()
        {
            var service = new OrderService();
            var cart = new Cart();
            // 50 TL'lik ürün → tam minimum tutar
            var product = new Product(1, "Ürün", 50m, 10);
            cart.AddItem(product, 1);

            var order = service.PlaceOrder(cart);

            // BUG: Bu test FAIL olacak çünkü if (total <= 50) ile 50 TL reddediliyor
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Confirmed),
                "BUG #5: Minimum sipariş tutarına tam eşit sipariş kabul edilmeli. " +
                "OrderService'te '<=' yerine '<' kullanılmalı.");
        }

        // =====================================================
        // TEST 19 - GrayBox - BVA: Minimum tutarın 1 altı (49 TL)
        // =====================================================
        [Test]
        public void PlaceOrder_BelowMinimumAmount_ShouldFail()
        {
            var service = new OrderService();
            var cart = new Cart();
            var product = new Product(1, "Ucuz Ürün", 49m, 10);
            cart.AddItem(product, 1);

            var order = service.PlaceOrder(cart);

            Assert.That(order.Status, Is.EqualTo(OrderStatus.Failed));
            Assert.That(order.FailReason, Does.Contain("Minimum sipariş"));
        }

        // =====================================================
        // TEST 20 - GrayBox - BVA: Minimum tutarın 1 üstü (51 TL)
        // =====================================================
        [Test]
        public void PlaceOrder_AboveMinimumAmount_ShouldSucceed()
        {
            var service = new OrderService();
            var cart = new Cart();
            var product = new Product(1, "Ürün", 51m, 10);
            cart.AddItem(product, 1);

            var order = service.PlaceOrder(cart);

            Assert.That(order.Status, Is.EqualTo(OrderStatus.Confirmed));
        }

        // =====================================================
        // TEST 21 - GrayBox - BUG #6 TESPİTİ: Stok tam yeterli ama ReduceStock başarısız
        // Stok=5, talep=5 → ReduceStock false dönüyor → sipariş başarısız
        // =====================================================
        [Test]
        public void PlaceOrder_StockExactlyMatchesQuantity_ShouldSucceed_BUG()
        {
            var service = new OrderService();
            var cart = new Cart();
            var product = new Product(1, "Son Stok Ürün", 100m, 5);
            cart.AddItem(product, 5); // Stok = 5, talep = 5

            var order = service.PlaceOrder(cart);

            // BUG: Product.ReduceStock(5) → Stock > 5 → false → sipariş fail
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Confirmed),
                "BUG #6: Stok tam yettiğinde sipariş kabul edilmeli. " +
                "ReduceStock'taki '>' operatörü '>=' olmalı.");
        }

        // =====================================================
        // TEST 22 - GrayBox - EP: Stok yetersiz
        // =====================================================
        [Test]
        public void PlaceOrder_InsufficientStock_ShouldFail()
        {
            var service = new OrderService();
            var cart = new Cart();
            var product = new Product(1, "Az Stoklu", 100m, 2);
            cart.AddItem(product, 5);

            var order = service.PlaceOrder(cart);

            Assert.That(order.Status, Is.EqualTo(OrderStatus.Failed));
            Assert.That(order.FailReason, Does.Contain("Yetersiz stok"));
        }

        // =====================================================
        // TEST 23 - GrayBox - BUG #3 TESPİTİ: %60 indirim (max %50 olmalı)
        // OrderService MaxDiscountPercent=50 tanımlı ama kontrol etmiyor
        // =====================================================
        [Test]
        public void PlaceOrder_DiscountExceedsMax_ShouldFail_BUG()
        {
            var service = new OrderService();
            var cart = new Cart();
            var product = new Product(1, "Pahalı Ürün", 1000m, 10);
            cart.AddItem(product, 1);

            var order = service.PlaceOrder(cart, 60); // %60 indirim

            // BUG: MaxDiscountPercent kontrolü yapılmıyor, %60 bile kabul ediliyor
            // Ayrıca ApplyDiscount'taki bug yüzünden hesaplama da yanlış
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Failed),
                "BUG #3: %50'den büyük indirim reddedilmeli. " +
                "OrderService'te MaxDiscountPercent kontrolü eksik.");
        }
    }

    /// <summary>
    /// SHIPPING FEE & COUPON TESTS
    /// Yeni eklenen kargo ücreti ve kupon sistemi testleri.
    /// </summary>
    [TestFixture]
    public class ShippingAndCouponTests
    {
        // =====================================================
        // TEST 28 - BlackBox - EP: 200 TL üzeri ücretsiz kargo
        // =====================================================
        [Test]
        public void CalculateShippingFee_Above200_ShouldBeFree()
        {
            var cart = new Cart();
            cart.AddItem(new Product(1, "Laptop", 5000m, 10), 1);

            decimal fee = cart.CalculateShippingFee();

            Assert.That(fee, Is.EqualTo(0m), "200 TL üzeri kargo ücretsiz olmalı.");
        }

        // =====================================================
        // TEST 29 - BlackBox - BVA: Tam 200 TL - BUG #7 TESPİTİ
        // BUG: > 200 kullanılmış, >= 200 olmalı
        // =====================================================
        [Test]
        public void CalculateShippingFee_Exactly200_ShouldBeFree_BUG()
        {
            var cart = new Cart();
            cart.AddItem(new Product(1, "Ürün", 200m, 10), 1);

            decimal fee = cart.CalculateShippingFee();

            // BUG: 200 TL'de kargo ücreti alınıyor (> yerine >= olmalı)
            Assert.That(fee, Is.EqualTo(0m),
                "BUG #7: Tam 200 TL'de kargo ücretsiz olmalı. " +
                "CalculateShippingFee'de '>' yerine '>=' kullanılmalı.");
        }

        // =====================================================
        // TEST 30 - BlackBox - BVA: 199 TL - kargo ücreti olmalı
        // =====================================================
        [Test]
        public void CalculateShippingFee_Below200_ShouldChargeFee()
        {
            var cart = new Cart();
            cart.AddItem(new Product(1, "Ürün", 199m, 10), 1);

            decimal fee = cart.CalculateShippingFee();

            Assert.That(fee, Is.EqualTo(29.99m), "200 TL altında 29.99 TL kargo ücreti olmalı.");
        }

        // =====================================================
        // TEST 31 - Unit/WhiteBox - EP: Geçerli kupon
        // =====================================================
        [Test]
        public void CouponIsValid_ValidCoupon_ShouldReturnTrue()
        {
            var coupon = new Coupon("INDIRIM10", 10m, DateTime.Now.AddDays(30), 0);

            bool result = coupon.IsValid(100m);

            Assert.That(result, Is.True, "Geçerli kupon true dönmeli.");
        }

        // =====================================================
        // TEST 32 - Unit/WhiteBox - EP: Kullanılmış kupon
        // =====================================================
        [Test]
        public void CouponIsValid_UsedCoupon_ShouldReturnFalse()
        {
            var coupon = new Coupon("USED01", 10m, DateTime.Now.AddDays(30), 0);
            coupon.MarkAsUsed();

            bool result = coupon.IsValid(100m);

            Assert.That(result, Is.False, "Kullanılmış kupon false dönmeli.");
        }

        // =====================================================
        // TEST 33 - Unit/WhiteBox - EP: Süresi dolmuş kupon
        // =====================================================
        [Test]
        public void CouponIsValid_ExpiredCoupon_ShouldReturnFalse()
        {
            var coupon = new Coupon("EXPIRED", 10m, DateTime.Now.AddDays(-1), 0);

            bool result = coupon.IsValid(100m);

            Assert.That(result, Is.False, "Süresi dolmuş kupon false dönmeli.");
        }

        // =====================================================
        // TEST 34 - GrayBox - BUG #8 TESPİTİ: MinOrderAmount kontrolü eksik
        // Kuponun minimum sipariş tutarı kontrolü yapılmıyor
        // =====================================================
        [Test]
        public void CouponIsValid_BelowMinOrderAmount_ShouldReturnFalse_BUG()
        {
            // Kupon: min 500 TL sipariş gerekli
            var coupon = new Coupon("MIN500", 15m, DateTime.Now.AddDays(30), 500m);

            // 100 TL'lik sipariş — 500 TL minimumun altında
            bool result = coupon.IsValid(100m);

            // BUG: MinOrderAmount kontrolü yok, 100 TL'de bile geçerli sayılıyor
            Assert.That(result, Is.False,
                "BUG #8: Minimum sipariş tutarının altında kupon geçersiz olmalı. " +
                "Coupon.IsValid'de MinOrderAmount kontrolü eksik.");
        }

        // =====================================================
        // TEST 35 - Integration: Kupon ile sipariş akışı
        // =====================================================
        [Test]
        public void PlaceOrderWithCoupon_ValidCoupon_ShouldApplyDiscount()
        {
            var service = new OrderService();
            var cart = new Cart();
            cart.AddItem(new Product(1, "Ürün", 1000m, 10), 1);
            var coupon = new Coupon("SAVE10", 10m, DateTime.Now.AddDays(30), 0);

            var order = service.PlaceOrderWithCoupon(cart, coupon);

            // Not: ApplyDiscount'taki BUG #4 yüzünden indirim yanlış hesaplanacak
            // ama kupon sistemi kendi başına çalışıyor
            Assert.That(coupon.IsUsed, Is.True, "Kupon kullanıldı olarak işaretlenmeli.");
            Assert.That(order.CouponCode, Is.EqualTo("SAVE10"));
        }

        // =====================================================
        // TEST 36 - Integration: Geçersiz kupon ile sipariş reddi
        // =====================================================
        [Test]
        public void PlaceOrderWithCoupon_ExpiredCoupon_ShouldFail()
        {
            var service = new OrderService();
            var cart = new Cart();
            cart.AddItem(new Product(1, "Ürün", 1000m, 10), 1);
            var coupon = new Coupon("OLD", 20m, DateTime.Now.AddDays(-5), 0);

            var order = service.PlaceOrderWithCoupon(cart, coupon);

            Assert.That(order.Status, Is.EqualTo(OrderStatus.Failed));
            Assert.That(order.FailReason, Does.Contain("Kupon geçersiz"));
        }
    }
}
