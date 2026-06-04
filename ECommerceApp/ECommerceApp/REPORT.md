# 📊 E-Ticaret Sistemi — Yazılım Test Raporu

**Proje:** ECommerceApp - E-Ticaret Test Projesi (Final)  
**Dil/Framework:** C# / .NET 8.0 / NUnit 4.x  
**Tarih:** Haziran 2026  

---

## 1. STLC (Software Testing Life Cycle) Süreci

### 1.1 Requirement (Gereksinim Analizi)

Sistemin fonksiyonel gereksinimleri şu şekilde belirlenmiştir:

| ID | Gereksinim | Öncelik |
|----|-----------|---------|
| REQ-01 | Kullanıcı ürün seçebilmeli | Yüksek |
| REQ-02 | Kullanıcı sepete ürün ekleyebilmeli | Yüksek |
| REQ-03 | Kullanıcı sepetten ürün çıkarabilmeli | Orta |
| REQ-04 | Sipariş verilebilmeli | Yüksek |
| REQ-05 | Ödeme işlemi yapılabilmeli | Yüksek |
| REQ-06 | Stokta olmayan ürün sipariş edilememeli | Yüksek |
| REQ-07 | İndirim (discount) uygulanabilmeli, max %50 | Orta |
| REQ-08 | Minimum sipariş tutarı 50 TL olmalı | Orta |
| REQ-09 | Sipariş iptal edildiğinde stok geri yüklenmeli | Orta |
| REQ-10 | Negatif fiyatlı veya geçersiz ürün oluşturulamamalı | Düşük |
| REQ-11 | 200 TL ve üzeri siparişlerde kargo ücretsiz olmalı | Orta |
| REQ-12 | Kupon kodu sistemi ile indirim uygulanabilmeli | Orta |
| REQ-13 | Kullanılmış veya süresi dolmuş kupon kabul edilmemeli | Orta |
| REQ-14 | Kuponun minimum sipariş tutarı kontrolü yapılmalı | Düşük |

### 1.2 Test Plan

**Kapsam:** Tüm Core sınıfları (Product, Cart, Coupon, OrderService) test edilecektir.

**Test Türleri:**
- Unit Test (White Box): İç kod yapısı bilinerek, metot seviyesinde test.
- Black Box Test: Sadece giriş/çıkış davranışına göre test.
- Gray Box Test: Kısmi iç bilgi ile (hata mesajları, sabitler bilinerek) test.
- Integration Test: Birden fazla bileşenin birlikte çalışmasını doğrulama.

**Test Dizayn Teknikleri:**
- Equivalence Partitioning (EP): Giriş değerleri geçerli/geçersiz gruplara ayrılır.
- Boundary Value Analysis (BVA): Sınır değerlerde (0, 1, min, max, min-1, max+1) test yapılır.

**Araçlar:** NUnit 4.x, .NET 8.0, dotnet test CLI.

**Hedef:** Minimum 20 test case, dengeli dağılım. Gerçekleşen: 36 test case.

### 1.3 Test Design

Her test case aşağıdaki bilgileri içerir:
- Test ID ve adı
- Test türü (Unit/BlackBox/GrayBox/Integration)
- Kullanılan teknik (EP veya BVA)
- Giriş değerleri
- Beklenen çıkış
- Gerçek çıkış
- Sonuç (Pass/Fail)

### 1.4 Test Execution

Testler `dotnet test` komutu ile NUnit framework üzerinde çalıştırılmıştır.

```bash
cd ECommerceApp
dotnet test --verbosity normal
```

### 1.5 Test Result & Reporting

Sonuçlar bu raporun devamında detaylı şekilde sunulmaktadır.

---

## 2. Test Türleri ve Açıklamaları

### 2.1 Unit Test (White Box)
Kodun iç yapısı (if/else dalları, döngüler, metot mantığı) bilinerek yazılır. Her metot bağımsız olarak test edilir. Amaç: Kodun her dalını (branch) en az bir kez çalıştırarak tüm mantık yollarını kapsamaktır.

**Bu projede:** `Product.ReduceStock()`, `Product.IsValid()`, `Coupon.IsValid()` metotlarının her dalı ayrı ayrı test edilmiştir.

### 2.2 Black Box Test
Kodun iç yapısı bilinmeden, sadece spesifikasyona göre giriş verilir ve beklenen çıkış kontrol edilir. Test eden kişi, fonksiyonun nasıl çalıştığını değil, ne yapması gerektiğini bilir.

**Bu projede:** Cart sınıfının AddItem, RemoveItem, ApplyDiscount ve CalculateShippingFee metotları sadece girdi/çıktı çiftleriyle test edilmiştir.

### 2.3 Gray Box Test
İç yapı kısmen bilinir (örn: hata mesajları, sabitler, veritabanı şeması). Hem davranış hem de bazı iç detaylar test edilir. White Box ve Black Box'un birleşimidir.

**Bu projede:** OrderService'in `MinimumOrderAmount = 50` ve `MaxDiscountPercent = 50` sabitlerini bilerek bu sınır değerlere yönelik testler yazılmıştır.

### 2.4 Integration Test
Birden fazla bileşenin (Product + Cart + Coupon + OrderService) bir arada doğru çalışıp çalışmadığını doğrular. Birimler tek tek çalışsa bile entegre olduğunda sorun çıkabilir.

**Bu projede:** Ürün seçimi → sepete ekleme → sipariş → ödeme → stok güncelleme akışının uçtan uca testi, kupon ile sipariş akışı ve çoklu sipariş stok tutarlılığı test edilmiştir.

---

## 3. Test Case Dizayn Teknikleri

### 3.1 Equivalence Partitioning (EP) Sınıfları

Bu teknikte giriş alanı, aynı davranışı sergileyen eşdeğer gruplara bölünür. Her gruptan bir temsilci seçilerek test edilir.

| Girdi | Geçerli Partition | Geçersiz Partition |
|-------|------------------|--------------------|
| Ürün miktarı (quantity) | 1 – Stock | ≤ 0, > Stock |
| İndirim yüzdesi | 0 – 50 | < 0, > 50 |
| Sipariş tutarı | ≥ 50 TL | < 50 TL |
| Ürün referansı | Geçerli Product nesnesi | null |
| Kargo eşiği | ≥ 200 TL | < 200 TL |
| Kupon durumu | Geçerli, süresi dolmamış, kullanılmamış | Süresi dolmuş, kullanılmış, boş kod |

### 3.2 Boundary Value Analysis (BVA) Sınır Değerleri

Bu teknikte sınır değerler (min, min+1, max, max+1, nominal) test edilir çünkü hataların büyük çoğunluğu sınır değerlerde ortaya çıkar.

| Parametre | Min-1 | Min | Min+1 | Nominal | Max-1 | Max | Max+1 |
|-----------|-------|-----|-------|---------|-------|-----|-------|
| Quantity | 0 | 1 | 2 | Stock/2 | Stock-1 | Stock | Stock+1 |
| Discount % | -1 | 0 | 1 | 25 | 49 | 50 | 51 |
| Sipariş Tutarı | 49 | 50 | 51 | 100 | — | — | — |
| Kargo Eşiği | 199 | 200 | 201 | 500 | — | — | — |

---

## 4. Detaylı Test Case Tablosu (36 Test)

| # | Test Adı | Tür | Teknik | Girdi | Beklenen | Gerçek | Sonuç |
|---|----------|------|--------|-------|----------|--------|-------|
| 1 | ReduceStock_ValidQuantity | Unit/WhiteBox | EP-Geçerli | Stock=10, qty=3 | true, Stock=7 | true, Stock=7 | ✅ PASS |
| 2 | ReduceStock_QuantityEqualsStock | Unit/WhiteBox | BVA-Sınır | Stock=5, qty=5 | true, Stock=0 | false | ❌ FAIL |
| 3 | ReduceStock_ZeroQuantity | Unit/WhiteBox | EP-Geçersiz | Stock=10, qty=0 | false | false | ✅ PASS |
| 4 | ReduceStock_NegativeQuantity | Unit/WhiteBox | EP-Geçersiz | Stock=10, qty=-1 | false | false | ✅ PASS |
| 5 | ReduceStock_ExceedsStock | Unit/WhiteBox | BVA-Sınır | Stock=3, qty=4 | false | false | ✅ PASS |
| 6 | IsValid_NegativePrice | Unit/WhiteBox | EP-Geçersiz | Price=-100 | false | true | ❌ FAIL |
| 7 | IsValid_ValidProduct | Unit/WhiteBox | EP-Geçerli | Name="Laptop", Price=15000 | true | true | ✅ PASS |
| 8 | AddItem_ValidProduct | BlackBox | EP-Geçerli | Product OK, qty=2 | true, count=2 | true, count=2 | ✅ PASS |
| 9 | AddItem_NullProduct | BlackBox | EP-Geçersiz | null, qty=1 | false | false | ✅ PASS |
| 10 | AddItem_ZeroQuantity | BlackBox | BVA-Sınır | Product OK, qty=0 | false | false | ✅ PASS |
| 11 | AddItem_SameProductTwice | BlackBox | EP-Geçerli | Aynı ürün 2+3 | count=5, 1 satır | count=5, 1 satır | ✅ PASS |
| 12 | RemoveItem_Existing | BlackBox | EP-Geçerli | Mevcut ürün ID | true, boş sepet | true, boş sepet | ✅ PASS |
| 13 | RemoveItem_NonExisting | BlackBox | EP-Geçersiz | ID=999 | false | false | ✅ PASS |
| 14 | ApplyDiscount_TenPercent | BlackBox | EP-Geçerli | Total=100, %10 | 90 TL | -900 TL | ❌ FAIL |
| 15 | ApplyDiscount_ZeroPercent | BlackBox | BVA-AltSınır | Total=200, %0 | 200 TL | 200 TL | ✅ PASS |
| 16 | ApplyDiscount_NegativePercent | BlackBox | EP-Geçersiz | Total=200, %-5 | 200 TL | 200 TL | ✅ PASS |
| 17 | PlaceOrder_EmptyCart | GrayBox | EP-Geçersiz | Boş sepet | Failed | Failed | ✅ PASS |
| 18 | PlaceOrder_ExactMinimum | GrayBox | BVA-Sınır | Total=50 TL | Confirmed | Failed | ❌ FAIL |
| 19 | PlaceOrder_BelowMinimum | GrayBox | BVA-Sınır | Total=49 TL | Failed | Failed | ✅ PASS |
| 20 | PlaceOrder_AboveMinimum | GrayBox | BVA-Sınır | Total=51 TL | Confirmed | Confirmed | ✅ PASS |
| 21 | PlaceOrder_StockExactMatch | GrayBox | BVA-Sınır | Stock=5, qty=5 | Confirmed | Failed | ❌ FAIL |
| 22 | PlaceOrder_InsufficientStock | GrayBox | EP-Geçersiz | Stock=2, qty=5 | Failed | Failed | ✅ PASS |
| 23 | PlaceOrder_DiscountExceedsMax | GrayBox | BVA-Sınır | %60 indirim | Failed | Confirmed | ❌ FAIL |
| 24 | FullOrderFlow | Integration | EP-Geçerli | Laptop+Mouse | Confirmed+Paid | Confirmed+Paid | ✅ PASS |
| 25 | CancelOrder_RestoreStock | Integration | EP-Geçerli | İptal sonrası stok | Stok geri yüklenir | Stok geri yüklenir | ✅ PASS |
| 26 | ProcessPayment_Insufficient | Integration | BVA-Sınır | 999 < 1000 TL | false | false | ✅ PASS |
| 27 | MultipleOrders_StockConsistency | Integration | EP-Geçerli | 3+4+5 adet | İlk 2 OK, 3. fail | İlk 2 OK, 3. fail | ✅ PASS |
| 28 | ShippingFee_Above200 | BlackBox | EP-Geçerli | Total=5000 TL | 0 TL (ücretsiz) | 0 TL | ✅ PASS |
| 29 | ShippingFee_Exactly200 | BlackBox | BVA-Sınır | Total=200 TL | 0 TL (ücretsiz) | 29.99 TL | ❌ FAIL |
| 30 | ShippingFee_Below200 | BlackBox | BVA-Sınır | Total=199 TL | 29.99 TL | 29.99 TL | ✅ PASS |
| 31 | Coupon_Valid | Unit/WhiteBox | EP-Geçerli | Geçerli kupon | true | true | ✅ PASS |
| 32 | Coupon_Used | Unit/WhiteBox | EP-Geçersiz | Kullanılmış kupon | false | false | ✅ PASS |
| 33 | Coupon_Expired | Unit/WhiteBox | EP-Geçersiz | Süresi dolmuş kupon | false | false | ✅ PASS |
| 34 | Coupon_BelowMinOrder | GrayBox | EP-Geçersiz | MinOrder=500, total=100 | false | true | ❌ FAIL |
| 35 | PlaceOrderWithCoupon_Valid | Integration | EP-Geçerli | Geçerli kupon | Confirmed, kupon used | Confirmed, kupon used | ✅ PASS |
| 36 | PlaceOrderWithCoupon_Expired | Integration | EP-Geçersiz | Süresi dolmuş kupon | Failed | Failed | ✅ PASS |

---

## 5. Test Summary

| Metrik | Değer |
|--------|-------|
| **Toplam Test** | 36 |
| **Başarılı (Passed)** | 28 |
| **Başarısız (Failed)** | 8 |
| **Başarı Oranı** | %77.8 |

### Test Türlerine Göre Dağılım

| Tür | Toplam | Pass | Fail |
|-----|--------|------|------|
| Unit Test (White Box) | 10 | 7 | 3 |
| Black Box Test | 12 | 10 | 2 |
| Gray Box Test | 8 | 4 | 4 |
| Integration Test | 6 | 6 | 0* |
| **TOPLAM** | **36** | **28** | **8** |

*Not: Integration testler bug'sız senaryoları kapsadığı için hepsi pass olmuştur.

### Teknik Dağılım

| Teknik | Kullanım Sayısı |
|--------|----------------|
| Equivalence Partitioning (Geçerli) | 13 |
| Equivalence Partitioning (Geçersiz) | 11 |
| Boundary Value Analysis | 12 |

---

## 6. Failed Tests ve Nedenleri

### ❌ FAIL #1 — Test 2: ReduceStock_QuantityEqualsStock
- **Beklenen:** `true` (stok düşürülmeli)
- **Gerçek:** `false`
- **Neden:** `Product.ReduceStock()` metodunda `Stock > quantity` kullanılmış. `Stock == quantity` durumunda (5 > 5 = false) stok düşürülemiyor.
- **Düzeltme:** `Stock > quantity` → `Stock >= quantity`

### ❌ FAIL #2 — Test 6: IsValid_NegativePrice
- **Beklenen:** `false` (negatif fiyat geçersiz olmalı)
- **Gerçek:** `true`
- **Neden:** `Product.IsValid()` metodunda `Price > 0` kontrolü bulunmuyor. Sadece `Name` ve `Stock` kontrol ediliyor.
- **Düzeltme:** `return !string.IsNullOrEmpty(Name) && Price > 0 && Stock >= 0;`

### ❌ FAIL #3 — Test 14: ApplyDiscount_TenPercent
- **Beklenen:** `90 TL` (%10 indirimle 100 TL → 90 TL)
- **Gerçek:** `-900 TL`
- **Neden:** `Cart.ApplyDiscount()` metodunda `discountPercent` değeri 100'e bölünmemiş. `total * discountPercent` hesaplanıyor, `total * (discountPercent / 100)` olması gerekirdi. 100 × 10 = 1000 çıkartılıyor.
- **Düzeltme:** `decimal discountAmount = total * discountPercent;` → `decimal discountAmount = total * (discountPercent / 100m);`

### ❌ FAIL #4 — Test 18: PlaceOrder_ExactMinimumAmount
- **Beklenen:** `OrderStatus.Confirmed` (50 TL tam sınırda kabul edilmeli)
- **Gerçek:** `OrderStatus.Failed`
- **Neden:** `OrderService.PlaceOrder()` metodunda `if (total <= MinimumOrderAmount)` kullanılmış. 50 TL tam sınır değerde sipariş reddediliyor.
- **Düzeltme:** `if (total <= MinimumOrderAmount)` → `if (total < MinimumOrderAmount)`

### ❌ FAIL #5 — Test 21: PlaceOrder_StockExactlyMatchesQuantity
- **Beklenen:** `OrderStatus.Confirmed`
- **Gerçek:** `OrderStatus.Failed` ("Stok düşürme başarısız")
- **Neden:** `Product.ReduceStock()` metodundaki BUG #1'in zincirleme etkisi. Stok tam yeterli olduğunda (Stock == Quantity) `ReduceStock` false döndüğü için sipariş başarısız oluyor.
- **Düzeltme:** BUG #1'in düzeltilmesi (ReduceStock'ta `>` → `>=`) bu sorunu da çözer.

### ❌ FAIL #6 — Test 23: PlaceOrder_DiscountExceedsMax
- **Beklenen:** `OrderStatus.Failed` (%60 > %50 max limit)
- **Gerçek:** `OrderStatus.Confirmed` (indirim limitsiz uygulanıyor)
- **Neden:** `OrderService.PlaceOrder()` metodunda `MaxDiscountPercent` sabiti tanımlı olmasına rağmen hiçbir yerde kontrol edilmiyor. %100 bile uygulanabilir durumda.
- **Düzeltme:** Sipariş vermeden önce `if (discountPercent > MaxDiscountPercent)` kontrolü eklenmeli.

### ❌ FAIL #7 — Test 29: CalculateShippingFee_Exactly200
- **Beklenen:** `0 TL` (200 TL'de kargo ücretsiz)
- **Gerçek:** `29.99 TL`
- **Neden:** `Cart.CalculateShippingFee()` metodunda `if (total > 200)` kullanılmış. 200 TL tam sınırda kargo ücreti alınıyor.
- **Düzeltme:** `if (total > 200m)` → `if (total >= 200m)`

### ❌ FAIL #8 — Test 34: CouponIsValid_BelowMinOrderAmount
- **Beklenen:** `false` (minimum sipariş tutarı altında kupon geçersiz)
- **Gerçek:** `true`
- **Neden:** `Coupon.IsValid()` metodunda `MinOrderAmount` kontrolü hiç yapılmıyor. Kupon, minimum tutar şartı sağlanmasa bile geçerli sayılıyor.
- **Düzeltme:** `if (orderTotal < MinOrderAmount) return false;` satırı eklenmeli.

---

## 7. Tespit Edilen Bug Listesi

| Bug ID | Sınıf | Metot | Açıklama | Severity | Tespit Eden Test |
|--------|-------|-------|----------|----------|-----------------|
| BUG-001 | Product | ReduceStock() | `>` operatörü `>=` olmalı — stok tam yettiğinde düşürme başarısız | 🔴 Yüksek | Test 2 |
| BUG-002 | Product | IsValid() | `Price > 0` kontrolü eksik — negatif fiyat geçerli sayılıyor | 🟡 Orta | Test 6 |
| BUG-003 | OrderService | PlaceOrder() | MaxDiscountPercent kontrolü yapılmıyor — %100 bile uygulanabilir | 🔴 Yüksek | Test 23 |
| BUG-004 | Cart | ApplyDiscount() | `discountPercent` 100'e bölünmemiş — hesaplama tamamen yanlış | 🔴 Kritik | Test 14 |
| BUG-005 | OrderService | PlaceOrder() | `<=` yerine `<` olmalı — 50 TL sınırda reddediliyor | 🟡 Orta | Test 18 |
| BUG-006 | OrderService | PlaceOrder() | BUG-001'in zincirleme etkisi — stok düşürme fail | 🔴 Yüksek | Test 21 |
| BUG-007 | Cart | CalculateShippingFee() | `>` yerine `>=` olmalı — 200 TL'de kargo ücreti alınıyor | 🟡 Orta | Test 29 |
| BUG-008 | Coupon | IsValid() | MinOrderAmount kontrolü eksik — minimum altında da geçerli | 🟡 Orta | Test 34 |

---

## 8. Hata Kavramları (Error, Fault, Failure, Defect/Bug)

> ⚠️ **Bu bölüm zorunlu olup, yazılım testindeki 4 temel hata kavramını tanımlar ve projeden somut örneklerle açıklar.**

### 8.1 Error (Hata / İnsan Yanılgısı)

**Tanım:** Geliştiricinin düşünce, anlama veya kodlama sürecinde yaptığı **insan hatası**dır. Henüz koda yansımamış olabilir; hatanın kaynağıdır. Bir yanlış anlama, dikkatsizlik veya bilgi eksikliğinden kaynaklanır.

**Projeden Örnek 1:** Geliştirici, indirim hesaplarken `%10` ifadesinin matematiksel olarak `0.10` çarpanına karşılık geldiğini unutmuş ve doğrudan `10` ile çarpmıştır. Bu bir **düşünce hatası / dikkatsizlik**tir.

**Projeden Örnek 2:** Geliştirici, `>` (büyüktür) ve `>=` (büyük eşittir) operatörlerinin farkını anlık olarak karıştırmış ve stok kontrolünde yanlış operatör kullanmıştır.

**Projeden Örnek 3:** Geliştirici, Coupon sınıfında MinOrderAmount özelliğini property olarak tanımlamış ama IsValid metoduna bu kontrolü eklemeyi unutmuştur. Bu bir **unutma / eksiklik** hatasıdır.

---

### 8.2 Fault (Kusur / Kod Hatası)

**Tanım:** Error sonucunda **kodda somut olarak oluşan yanlış yapı**dır. Kaynak kodda gözlemlenebilir, statik bir kusurun kendisidir. Bir fault, henüz çalıştırılmadıysa failure'a yol açmaz.

**Projeden Örnek 1 — İndirim hesaplama kusuru:**
```csharp
// FAULT: 100'e bölme eksik
decimal discountAmount = total * discountPercent;
// DOĞRUSU:
decimal discountAmount = total * (discountPercent / 100m);
```

**Projeden Örnek 2 — Stok karşılaştırma kusuru:**
```csharp
// FAULT: >= yerine > kullanılmış
if (quantity > 0 && Stock > quantity)
// DOĞRUSU:
if (quantity > 0 && Stock >= quantity)
```

**Projeden Örnek 3 — Eksik kontrol kusuru:**
```csharp
// FAULT: MinOrderAmount kontrolü hiç yok
public bool IsValid(decimal orderTotal)
{
    // ... diğer kontroller var ama bu satır eksik:
    // if (orderTotal < MinOrderAmount) return false;
    return true;
}
```

---

### 8.3 Failure (Başarısızlık / Yanlış Çalışma)

**Tanım:** Fault'un **çalışma zamanında** tetiklenmesi sonucu gözlemlenen **yanlış davranış**tır. Sistemin beklenen çıktıdan sapmasıdır. Bir failure, test çalıştırıldığında veya kullanıcı sistemi kullandığında ortaya çıkar.

**Projeden Örnek 1:** `%10` indirim uygulandığında `100 TL`'lik ürün için beklenen sonuç `90 TL` iken, sistem **`-900 TL`** döndü. Kullanıcıya negatif tutar gösterildi → **gözlemlenen failure**.

**Projeden Örnek 2:** Stokta 5 adet kalan ürünü 5 adet sipariş etmeye çalışan kullanıcı, "Stok düşürme başarısız" hatası aldı. Stok yetmesine rağmen sipariş reddedildi → **gözlemlenen failure**.

**Projeden Örnek 3:** Sepet toplamı tam 200 TL olan kullanıcıya "ücretsiz kargo" vaadine rağmen 29.99 TL kargo ücreti yansıtıldı → **gözlemlenen failure**.

---

### 8.4 Defect / Bug (Raporlanmış Hata)

**Tanım:** Test sürecinde tespit edilip **resmi olarak kaydedilen ve takip edilen** sorun. Bir bug raporu olarak belgelenir; severity (önem derecesi), öncelik, adımlar ve düzeltme önerisi içerir.

**Projeden Örnek 1:** **BUG-004** — "İndirim hesaplama formülü hatalı". Test #14 tarafından tespit edilmiş, severity **Kritik** olarak sınıflandırılmış. Düzeltme önerisi: `discountPercent` değerinin `100m`'ye bölünmesi.

**Projeden Örnek 2:** **BUG-007** — "Kargo ücreti sınır değer hatası". Test #29 tarafından tespit edilmiş, severity **Orta** olarak sınıflandırılmış. Düzeltme önerisi: `>` operatörünün `>=` olarak değiştirilmesi.

**Projeden Örnek 3:** **BUG-008** — "Kupon minimum sipariş kontrolü eksik". Test #34 tarafından tespit edilmiş. Düzeltme önerisi: `if (orderTotal < MinOrderAmount) return false;` eklenmesi.

---

### 8.5 Kavramlar Arası İlişki ve Akış

```
  ┌──────────┐      ┌──────────┐      ┌──────────┐      ┌──────────┐
  │  ERROR   │ ───→ │  FAULT   │ ───→ │ FAILURE  │ ───→ │ DEFECT/  │
  │ (İnsan)  │      │ (Kodda)  │      │ (Çalışma │      │   BUG    │
  │          │      │          │      │  zamanı) │      │(Raporlan-│
  │ Düşünce  │      │ Yanlış   │      │ Yanlış   │      │  mış)    │
  │ hatası   │      │ satır    │      │ çıktı    │      │          │
  └──────────┘      └──────────┘      └──────────┘      └──────────┘
```

**Somut akış örneği (BUG-004):**

| Aşama | Açıklama | Projeden Örnek |
|-------|----------|---------------|
| **Error** | Geliştirici yüzdeyi orana çevirmeyi unuttu | `%10` → `0.10` dönüşümü atlandı |
| **Fault** | Kodda `total * discountPercent` yazıldı | `100 * 10 = 1000` çıkartılacak |
| **Failure** | Test çalışınca beklenen 90 yerine -900 döndü | Kullanıcı negatif tutar gördü |
| **Defect/Bug** | BUG-004 olarak raporlandı, Kritik severity | Test #14 ile tespit edildi |

**Önemli not:** Tüm fault'lar failure'a yol açmaz. Eğer ilgili kod yolu hiç çalıştırılmazsa (örneğin, hiç indirim uygulanmazsa) fault sessiz kalır. Testlerin amacı, olabildiğince çok fault'u tetikleyerek failure'ları ortaya çıkarmak ve defect olarak raporlamaktır.

**Zincirleme etki örneği:** BUG-001 (Error: operatör karışıklığı → Fault: `>` yerine `>=` → Failure: stok düşürme başarısız) doğrudan BUG-006'ya neden olmuştur (Failure: sipariş tamamlanamıyor). Tek bir error, birden fazla failure'a yol açabilir.

---

## 9. Test Stratejileri

> Aşağıda projede uygulanan 3 temel test stratejisi **ayrı başlıklar** halinde açıklanmıştır.

### 9.1 Agile Testing

**Tanım:** Agile metodolojide test, geliştirme sürecinin **ayrılmaz bir parçası**dır. Geleneksel "önce geliştir, sonra test et" yaklaşımının aksine, her sprint/iterasyonda geliştirme ve test **paralel** yürütülür. Tüm ekip kaliteden sorumludur.

**Temel İlkeler:**
- Test, kodlamayla eş zamanlı yazılır (shift-left testing).
- Her iterasyon sonunda çalışan ve test edilmiş yazılım teslim edilir.
- Sürekli geri bildirim döngüsü ile hatalar erken yakalanır.
- Test otomasyon önemlidir; hızlı iterasyonlara ayak uydurulmalıdır.

**Bu projede uygulanışı:**
- Her sınıf geliştirilirken eş zamanlı olarak testleri yazılmıştır (Product geliştirildi → Product testleri yazıldı → Cart geliştirildi → Cart testleri yazıldı).
- 3 kısa iterasyonla ilerlendi:
  - **İterasyon 1:** Product + Cart + temel testler
  - **İterasyon 2:** OrderService + stok/indirim/minimum tutar testleri
  - **İterasyon 3:** Coupon + Shipping + integration testler
- TDD benzeri düşünce yapısıyla beklenen davranış önce tanımlanmış, ardından kod yazılmıştır.

---

### 9.2 Risk-Based Testing

**Tanım:** Test eforunu **riskin en yüksek olduğu alanlara** yoğunlaştıran stratejidir. Sınırlı zaman ve kaynak ile maksimum test coverage elde etmeyi hedefler. Risk = Olasılık × Etki formülü ile önceliklendirme yapılır.

**Temel İlkeler:**
- Yüksek iş etkisi olan modüller önce test edilir.
- Risk matrisi ile her alan puanlanır.
- Düşük riskli alanlara minimal test yazılır.
- Kaynak verimliliği sağlar.

**Bu projede uygulanışı:**

Risk matrisi oluşturulmuş ve test dağılımı buna göre belirlenmiştir:

| Alan | Olasılık | Etki | Risk Skoru (OxE) | Test Sayısı | Test Yoğunluğu |
|------|---------|------|------------------|-------------|----------------|
| Stok kontrolü | Yüksek (3) | Yüksek (3) | **9** | 6 | Çok yüksek |
| İndirim hesaplama | Orta (2) | Yüksek (3) | **6** | 5 | Yüksek |
| Sipariş akışı | Orta (2) | Yüksek (3) | **6** | 6 | Yüksek |
| Kargo ücreti | Orta (2) | Orta (2) | **4** | 3 | Orta |
| Kupon sistemi | Orta (2) | Orta (2) | **4** | 6 | Orta |
| Minimum sipariş | Orta (2) | Orta (2) | **4** | 3 | Orta |
| Ürün validasyon | Düşük (1) | Düşük (1) | **1** | 2 | Düşük |

Bu strateji sayesinde en kritik bug (BUG-004, indirim hesaplama) yüksek test yoğunluğu ile erken tespit edilmiştir.

---

### 9.3 Regression Testing

**Tanım:** Bir değişiklik (bug fix, yeni özellik, refactoring) yapıldığında, **mevcut fonksiyonların bozulup bozulmadığını** kontrol eden stratejidir. "Bir şeyi düzeltirken başka bir şeyi bozmadık mı?" sorusuna cevap verir.

**Temel İlkeler:**
- Her değişiklik sonrası tüm test suite tekrar çalıştırılır.
- Otomasyon zorunludur; manuel regression çok maliyetlidir.
- CI/CD pipeline'a entegre edilmelidir.
- Eski testlerin tamamının pass kalması beklenir.

**Bu projede uygulanışı:**
- Tüm 36 test otomatize edilmiştir ve `dotnet test` ile tek komutla çalıştırılabilir.
- Herhangi bir bug fix sonrası tüm test suite tekrar çalıştırılarak regression kontrolü yapılır.

**Örnek regression senaryosu:**

```
BUG-001 düzeltildi (ReduceStock: > → >=)
↓
dotnet test çalıştırıldı
↓
Beklenen sonuç:
  ✅ Test 2 artık PASS olmalı (düzeltilen bug)
  ✅ Test 21 artık PASS olmalı (zincirleme etki çözülür)
  ✅ Test 1, 3, 4, 5 hâlâ PASS kalmalı (regression yok)
  
Eğer Test 1 FAIL olursa → REGRESSION tespit edildi!
```

- Yeni Coupon ve Shipping özellikleri eklendiğinde, mevcut 27 testin tamamı tekrar çalıştırılarak hiçbirinin bozulmadığı doğrulanmıştır.
- CI/CD pipeline'a entegre edilebilir yapıda (`dotnet test --logger trx`) tasarlanmıştır.

---

## 10. Sonuç ve Değerlendirme

Bu projede bir e-ticaret sisteminin temel fonksiyonları (ürün yönetimi, sepet, sipariş, ödeme, stok kontrolü, indirim, kargo ücreti, kupon sistemi) geliştirilmiş ve kapsamlı bir test süreci uygulanmıştır.

**Öne çıkan bulgular:**
- Toplam **8 bug** tespit edilmiştir ve bunlardan **1 tanesi kritik** seviyededir (indirim hesaplama hatası).
- BUG-001 zincirleme etkiyle BUG-006'ya neden olmaktadır; bu, tek bir fault'un birden fazla failure'a yol açabileceğini gösterir.
- **BVA** tekniği, sınır değerlerdeki hataları (BUG-001, BUG-005, BUG-007) tespit etmede EP'den daha etkili olmuştur.
- **EP** tekniği, eksik kontrolleri (BUG-002, BUG-003, BUG-008) tespit etmede kullanılmıştır.
- Integration testler, bileşenlerin bir arada doğru çalıştığını (bug'sız senaryolarda) doğrulamıştır.

**Öğrenilen dersler:**
- Karşılaştırma operatörlerinde (`>` vs `>=`, `<` vs `<=`) büyük dikkat gereklidir.
- Matematiksel formüllerde birim dönüşümleri (yüzde → oran) mutlaka doğrulanmalıdır.
- Sabit tanımlayıp kullanmamak (MaxDiscountPercent, MinOrderAmount) yaygın bir code smell'dir.
- Sınır değer testleri (BVA), hatların büyük çoğunluğunu ortaya çıkarmada en etkili tekniktir.
- Property tanımlayıp iş mantığında kullanmayı unutmak (Coupon.MinOrderAmount) sık yapılan bir eksikliktir.
