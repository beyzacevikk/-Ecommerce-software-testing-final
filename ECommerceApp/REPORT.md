# 📊 E-Ticaret Sistemi - Yazılım Test Raporu

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

### 1.2 Test Plan

**Kapsam:** Tüm Core sınıfları (Product, Cart, OrderService) test edilecektir.

**Test Türleri:**
- Unit Test (White Box): İç kod yapısı bilinerek, metot seviyesinde test.
- Black Box Test: Sadece giriş/çıkış davranışına göre test.
- Gray Box Test: Kısmi iç bilgi ile (hata mesajları, sabitler bilinerek) test.
- Integration Test: Birden fazla bileşenin birlikte çalışmasını doğrulama.

**Test Dizayn Teknikleri:**
- Equivalence Partitioning (EP): Giriş değerleri geçerli/geçersiz gruplara ayrılır.
- Boundary Value Analysis (BVA): Sınır değerlerde (0, 1, min, max, min-1, max+1) test yapılır.

**Araçlar:** NUnit 4.x, .NET 8.0, dotnet test CLI.

**Hedef:** Minimum 20 test case, dengeli dağılım.

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
Kodun iç yapısı (if/else dalları, döngüler, metot mantığı) bilinerek yazılır. Her metot bağımsız olarak test edilir. Örnek: `Product.ReduceStock()` metodunun her dalının ayrı ayrı test edilmesi.

### 2.2 Black Box Test
Kodun iç yapısı bilinmeden, sadece spesifikasyona göre giriş verilir ve beklenen çıkış kontrol edilir. Örnek: Sepete ürün ekleme fonksiyonunun sadece girdi/çıktı çiftleriyle test edilmesi.

### 2.3 Gray Box Test
İç yapı kısmen bilinir (örn: hata mesajları, sabitler, veritabanı şeması). Hem davranış hem de bazı iç detaylar test edilir. Örnek: OrderService'in minimum tutar kontrolünü MinimumOrderAmount sabitini bilerek test etme.

### 2.4 Integration Test
Birden fazla bileşenin (Product + Cart + OrderService) bir arada doğru çalışıp çalışmadığını doğrular. Örnek: Ürün seçimi → sepete ekleme → sipariş → ödeme → stok güncelleme akışının uçtan uca testi.

---

## 3. Test Case Tablosu

### Test Case Dizayn Teknikleri

**Equivalence Partitioning (EP) Sınıfları:**

| Girdi | Geçerli Partition | Geçersiz Partition |
|-------|------------------|--------------------|
| Ürün miktarı (quantity) | 1 – Stock | ≤ 0, > Stock |
| İndirim yüzdesi | 0 – 50 | < 0, > 50 |
| Sipariş tutarı | ≥ 50 TL | < 50 TL |
| Ürün referansı | Geçerli Product nesnesi | null |

**Boundary Value Analysis (BVA) Sınır Değerleri:**

| Parametre | Min | Min+1 | Nominal | Max-1 | Max | Max+1 |
|-----------|-----|-------|---------|-------|-----|-------|
| Quantity | 0 | 1 | Stock/2 | Stock-1 | Stock | Stock+1 |
| Discount % | -1 | 0 | 25 | 49 | 50 | 51 |
| Sipariş Tutarı | 49 | 50 | 100 | — | — | — |

### Detaylı Test Case Listesi (27 Test)

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

---

## 4. Test Summary

| Metrik | Değer |
|--------|-------|
| **Toplam Test** | 27 |
| **Başarılı (Passed)** | 21 |
| **Başarısız (Failed)** | 6 |
| **Başarı Oranı** | %77.8 |

### Test Türlerine Göre Dağılım

| Tür | Toplam | Pass | Fail |
|-----|--------|------|------|
| Unit Test (White Box) | 7 | 5 | 2 |
| Black Box Test | 9 | 7 | 2* |
| Gray Box Test | 7 | 4 | 3* |
| Integration Test | 4 | 4 | 0 |

*Not: Bazı fail'ler aynı root-cause bug'dan kaynaklanmaktadır.

### Teknik Dağılım

| Teknik | Kullanım Sayısı |
|--------|----------------|
| Equivalence Partitioning (Geçerli) | 10 |
| Equivalence Partitioning (Geçersiz) | 7 |
| Boundary Value Analysis | 10 |

---

## 5. Failed Tests ve Nedenleri

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

---

## 6. Tespit Edilen Bug Listesi

| Bug ID | Sınıf | Metot | Açıklama | Severity | Root Cause |
|--------|-------|-------|----------|----------|------------|
| BUG-001 | Product | ReduceStock() | Stok miktarı talebe tam eşitken düşürme başarısız | Yüksek | `>` operatörü `>=` olmalı |
| BUG-002 | Product | IsValid() | Negatif fiyat kontrolü eksik | Orta | `Price > 0` koşulu yok |
| BUG-003 | OrderService | PlaceOrder() | Maksimum indirim limiti kontrol edilmiyor | Yüksek | MaxDiscountPercent kontrolü eksik |
| BUG-004 | Cart | ApplyDiscount() | İndirim hesaplama formülü hatalı | Kritik | `discountPercent` 100'e bölünmemiş |
| BUG-005 | OrderService | PlaceOrder() | Minimum sipariş sınır değerde reddediliyor | Orta | `<=` yerine `<` olmalı |
| BUG-006 | OrderService | PlaceOrder() | Stok tam yettiğinde sipariş başarısız | Yüksek | BUG-001'in zincirleme etkisi |

---

## 7. Hata Kavramları (Error, Fault, Failure, Defect/Bug)

### 7.1 Error (Hata/Yanılgı)
Geliştiricinin düşünce veya kodlama sürecinde yaptığı insan hatası.

**Örnek:** Geliştirici, indirim hesaplarken yüzdelik değeri ondalığa çevirmeyi unutmuş. `%10` indirim için `0.10` çarpanı kullanması gerekirken `10` ile çarpmıştır. Bu bir insan düşünce hatasıdır.

### 7.2 Fault (Kusur)
Error sonucu kodda oluşan yanlış yapı. Kodda somut olarak gözlemlenebilen yanlış satır.

**Örnek:** `Cart.ApplyDiscount()` metodundaki satır:
```csharp
decimal discountAmount = total * discountPercent; // FAULT: 100'e bölme yok
```
Doğrusu: `decimal discountAmount = total * (discountPercent / 100m);`

### 7.3 Failure (Başarısızlık)
Fault'un çalışma zamanında gözlemlenen yanlış davranışı. Sistemin beklenen çıktıdan sapması.

**Örnek:** %10 indirim uygulandığında 100 TL'lik ürün için beklenen sonuç 90 TL iken, sistem -900 TL döndü. Bu gözlemlenen failure'dır.

### 7.4 Defect / Bug
Test sürecinde tespit edilip raporlanan hata. Resmi olarak kaydedilmiş ve takip edilen sorun.

**Örnek:** BUG-004 olarak raporlanan "İndirim hesaplama formülü hatalı" defect'i. Test #14 tarafından tespit edilmiş, severity "Kritik" olarak sınıflandırılmış ve düzeltme önerisi sunulmuştur.

### Kavramlar Arası İlişki (Akış)

```
Error (Düşünce hatası) → Fault (Kodda kusur) → Failure (Çalışmada hata) → Defect/Bug (Raporlanan sorun)
```

Tüm fault'lar failure'a yol açmaz (ilgili kod yolu çalıştırılmayabilir). Ancak testlerin amacı olabildiğince çok fault'u tetikleyerek failure'ları ortaya çıkarmak ve defect olarak raporlamaktır.

---

## 8. Test Stratejileri

### 8.1 Agile Testing
Agile metodolojide test, geliştirme sürecinin ayrılmaz bir parçasıdır. Her sprint'te geliştirme ve test paralel yürütülür. Bu projede Agile Testing yaklaşımı şu şekilde uygulanmıştır:
- Her sınıf geliştirilirken eş zamanlı olarak testleri yazılmıştır.
- Kısa iterasyonlarla (Product → Cart → OrderService) geliştirme ve test döngüsü tamamlanmıştır.
- Test-first (TDD benzeri) düşünce yapısıyla, beklenen davranış önce tanımlanmış, sonra kodlanmıştır.

### 8.2 Risk-Based Testing
Test eforunu riskin en yüksek olduğu alanlara yoğunlaştıran stratejidir. Bu projede:
- **Yüksek risk:** Ödeme ve stok işlemleri (parasal kayıp riski) → En çok test bu alanlara yazılmıştır.
- **Orta risk:** İndirim hesaplamaları (gelir kaybı riski) → Sınır değer testleri yoğun uygulanmıştır.
- **Düşük risk:** Ürün validasyonu → Temel EP testleri yeterli görülmüştür.

Risk matrisi:

| Alan | Olasılık | Etki | Risk Skoru | Test Yoğunluğu |
|------|---------|------|------------|----------------|
| Stok kontrolü | Yüksek | Yüksek | 9 | Çok yüksek |
| İndirim hesaplama | Orta | Yüksek | 6 | Yüksek |
| Minimum sipariş | Orta | Orta | 4 | Orta |
| Ürün validasyon | Düşük | Düşük | 2 | Düşük |

### 8.3 Regression Testing
Bir değişiklik yapıldığında mevcut fonksiyonların bozulup bozulmadığını kontrol eden stratejidir. Bu projede:
- Tüm 27 test otomatize edilmiştir ve `dotnet test` ile tek komutla çalıştırılabilir.
- Herhangi bir bug fix sonrası tüm test suite tekrar çalıştırılarak regression kontrolü yapılır.
- Örnek senaryo: BUG-001 düzeltildiğinde, Test 2 ve Test 21 pass olmalı, ancak Test 1, 3, 4, 5 gibi mevcut testler de hâlâ pass kalmalıdır.
- CI/CD pipeline'a entegre edilebilir yapıda tasarlanmıştır.

---

## 9. Sonuç ve Değerlendirme

Bu projede bir e-ticaret sisteminin temel fonksiyonları (ürün yönetimi, sepet işlemleri, sipariş verme, stok kontrolü, indirim uygulama, minimum sipariş kontrolü) geliştirilmiş ve kapsamlı bir test süreci uygulanmıştır.

**Öne çıkan bulgular:**
- Toplam 6 bug tespit edilmiştir ve bunlardan 1 tanesi kritik seviyededir (indirim hesaplama hatası).
- BUG-001 zincirleme etkiyle BUG-006'ya neden olmaktadır; bu durum, bir fault'un birden fazla failure'a yol açabileceğini göstermektedir.
- BVA tekniği, sınır değerlerdeki hataları (BUG-001, BUG-005) tespit etmede EP'den daha etkili olmuştur.
- Integration testler tüm bileşenlerin bir arada doğru çalıştığını (bug'sız senaryolarda) doğrulamıştır.

**Öğrenilen dersler:**
- Karşılaştırma operatörlerinde (`>` vs `>=`, `<` vs `<=`) dikkatli olunmalıdır.
- Matematiksel formüllerde birim dönüşümleri (yüzde → oran) mutlaka doğrulanmalıdır.
- Sabit tanımlayıp kullanmamak (MaxDiscountPercent) yaygın bir kod kusuru türüdür.
- Sınır değer testleri, birçok hatayı ortaya çıkarmada en etkili tekniktir.
