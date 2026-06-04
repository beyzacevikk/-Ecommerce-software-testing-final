# 🛒 E-Commerce Software Testing Project

> Yazılım Test ve Kalite Güvencesi Dersi — Final Projesi

## 📌 Proje Hakkında

Bu proje, bir **e-ticaret sisteminin** temel işlevlerini (ürün yönetimi, sepet, sipariş, ödeme) C# ile modellemekte ve **yazılım test süreçlerini** (STLC) uçtan uca uygulayarak göstermektedir.

Sistem içerisinde **bilerek bırakılmış 6 adet bug** bulunmaktadır. Bu hatalar, farklı test türleri ve test dizayn teknikleri kullanılarak **27 NUnit test case** ile tespit edilmektedir.

---

## 🏗️ Proje Yapısı

```
ECommerceApp/
│
├── Core/                              # İş mantığı katmanı
│   ├── Product.cs                     # Ürün sınıfı (stok yönetimi, validasyon)
│   ├── Cart.cs                        # Sepet sınıfı (ekleme, çıkarma, indirim, toplam)
│   └── OrderService.cs                # Sipariş servisi (sipariş, ödeme, iptal, stok kontrolü)
│
├── Tests/                             # Test katmanı
│   ├── UnitTests/
│   │   └── ECommerceTests.cs          # Unit (White Box) + Black Box + Gray Box testler
│   └── IntegrationTests/
│       └── OrderIntegrationTests.cs   # Integration testler (uçtan uca akışlar)
│
├── Program.cs                         # Demo konsol uygulaması
├── ECommerceApp.csproj                # .NET 8.0 + NUnit 4.x proje dosyası
├── REPORT.md                          # 📊 Detaylı test raporu
├── README.md                          # Bu dosya
└── .gitignore
```

---

## ⚙️ Teknolojiler

| Teknoloji | Versiyon | Kullanım |
|-----------|----------|----------|
| C# | 12.0 | Uygulama geliştirme |
| .NET | 8.0 | Runtime / SDK |
| NUnit | 4.1.0 | Test framework |
| NUnit3TestAdapter | 4.5.0 | Test runner |
| Microsoft.NET.Test.Sdk | 17.9.0 | Test altyapısı |

---

## 🔧 Kurulum ve Çalıştırma

### Gereksinimler
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) yüklü olmalıdır.

### 1. Repoyu Klonlayın
```bash
git clone https://github.com/KULLANICI_ADINIZ/ecommerce-software-testing-final.git
cd ecommerce-software-testing-final
```

### 2. Bağımlılıkları Yükleyin
```bash
dotnet restore
```

### 3. Projeyi Derleyin
```bash
dotnet build
```

### 4. Demo Uygulamayı Çalıştırın
```bash
dotnet run
```

### 5. Testleri Çalıştırın
```bash
dotnet test --verbosity normal
```

> ⚠️ **Beklenen sonuç:** 28 test PASS, 8 test FAIL olacaktır. Fail olan testler bilerek bırakılmış bug'ları tespit etmektedir.

---

## 🎯 Uygulanan Senaryolar

| Senaryo | Açıklama | Sınıf |
|---------|----------|-------|
| 🛍️ Ürün Seçimi | Ürün oluşturma, fiyat ve stok bilgisi | `Product.cs` |
| 🛒 Sepete Ekleme | Ürün ekleme/çıkarma, miktar birleştirme | `Cart.cs` |
| 📦 Sipariş Verme | Stok kontrolü, minimum tutar, indirim uygulama | `OrderService.cs` |
| 💳 Ödeme | Ödeme tutarı doğrulama | `OrderService.cs` |
| 📉 Stok Kontrolü | Yetersiz/sınır stokta sipariş engelleme | `Product.cs` + `OrderService.cs` |
| 🏷️ İndirim (Discount) | Yüzdelik indirim hesaplama, max limit | `Cart.cs` + `OrderService.cs` |
| 💰 Minimum Sipariş Tutarı | 50 TL altı siparişleri reddetme | `OrderService.cs` |
| ❌ Sipariş İptali | İptal sonrası stok geri yükleme | `OrderService.cs` |
| 🚚 Kargo Ücreti | 200 TL üzeri ücretsiz kargo hesaplama | `Cart.cs` |
| 🎟️ Kupon Kodu | Geçerli/geçersiz/süresi dolmuş kupon yönetimi | `Cart.cs` + `OrderService.cs` |

---

## 🐛 Bilerek Bırakılan Bug'lar (8 Adet)

| Bug ID | Konum | Açıklama | Severity |
|--------|-------|----------|----------|
| BUG-001 | `Product.ReduceStock()` | `Stock > quantity` yerine `Stock >= quantity` olmalı — stok tam yettiğinde sipariş başarısız | 🔴 Yüksek |
| BUG-002 | `Product.IsValid()` | `Price > 0` kontrolü eksik — negatif fiyatlı ürün geçerli sayılıyor | 🟡 Orta |
| BUG-003 | `OrderService.PlaceOrder()` | `MaxDiscountPercent` sabiti tanımlı ama kontrol edilmiyor — %100 indirim bile uygulanabiliyor | 🔴 Yüksek |
| BUG-004 | `Cart.ApplyDiscount()` | `discountPercent` değeri 100'e bölünmemiş — %10 indirim yerine %1000 uygulanıyor | 🔴 Kritik |
| BUG-005 | `OrderService.PlaceOrder()` | `<=` yerine `<` olmalı — 50 TL tam sınır sipariş reddediliyor | 🟡 Orta |
| BUG-006 | `OrderService.PlaceOrder()` | BUG-001'in zincirleme etkisi — stok düşürme başarısız olunca sipariş fail | 🔴 Yüksek |
| BUG-007 | `Cart.CalculateShippingFee()` | `>` yerine `>=` olmalı — 200 TL'de kargo ücreti alınıyor | 🟡 Orta |
| BUG-008 | `Coupon.IsValid()` | `MinOrderAmount` kontrolü eksik — minimum altında da kupon geçerli | 🟡 Orta |

---

## 🧪 Test Özeti

### Genel Sonuçlar

```
═══════════════════════════════════════
  Total:    36
  Passed:   28  ✅
  Failed:    8  ❌
  Başarı:   %77.8
═══════════════════════════════════════
```

### Test Türlerine Göre Dağılım

| Test Türü | Adet | Pass | Fail | Açıklama |
|-----------|------|------|------|----------|
| **Unit Test (White Box)** | 10 | 7 | 3 | Kodun iç yapısı bilinerek yazılmış |
| **Black Box Test** | 12 | 10 | 2 | Sadece giriş/çıkış davranışına göre |
| **Gray Box Test** | 8 | 4 | 4 | Kısmi iç bilgi ile (sabitler, mesajlar) |
| **Integration Test** | 6 | 6 | 0 | Bileşenlerin birlikte çalışma testi |
| **TOPLAM** | **36** | **28** | **8** | — |

### Kullanılan Test Dizayn Teknikleri

| Teknik | Kullanım | Örnek |
|--------|---------|-------|
| **Equivalence Partitioning (EP)** | 24 test | Geçerli: qty=3, Geçersiz: qty=0, qty=-1, null, expired coupon |
| **Boundary Value Analysis (BVA)** | 12 test | qty=Stock, total=49/50/51 TL, shipping=199/200/201 TL |

### Failed Test Listesi

| # | Test Adı | Beklenen | Gerçek | İlgili Bug |
|---|----------|----------|--------|------------|
| 2 | `ReduceStock_QuantityEqualsStock` | true | false | BUG-001 |
| 6 | `IsValid_NegativePrice` | false | true | BUG-002 |
| 14 | `ApplyDiscount_TenPercent` | 90 TL | -900 TL | BUG-004 |
| 18 | `PlaceOrder_ExactMinimumAmount` | Confirmed | Failed | BUG-005 |
| 21 | `PlaceOrder_StockExactlyMatchesQuantity` | Confirmed | Failed | BUG-006 |
| 23 | `PlaceOrder_DiscountExceedsMax` | Failed | Confirmed | BUG-003 |
| 29 | `ShippingFee_Exactly200` | 0 TL | 29.99 TL | BUG-007 |
| 34 | `Coupon_BelowMinOrderAmount` | false | true | BUG-008 |

---

## 🔁 STLC (Software Testing Life Cycle) Süreci

```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│ Requirement │ ─→ │  Test Plan  │ ─→ │ Test Design │ ─→ │  Execution  │ ─→ │  Reporting  │
│   Analizi   │    │  Hazırlama  │    │  Case Yazım │    │ Test Çalış. │    │   Raporlama │
└─────────────┘    └─────────────┘    └─────────────┘    └─────────────┘    └─────────────┘
```

1. **Requirement:** 10 fonksiyonel gereksinim tanımlandı
2. **Test Plan:** Test türleri, araçlar ve hedef test sayısı belirlendi
3. **Test Design:** EP ve BVA teknikleriyle 27 test case tasarlandı
4. **Test Execution:** `dotnet test` ile NUnit üzerinde çalıştırıldı
5. **Test Reporting:** Sonuçlar `REPORT.md` dosyasında raporlandı

---

## ⚠️ Hata Kavramları

| Kavram | Tanım | Projeden Örnek |
|--------|-------|---------------|
| **Error** | Geliştiricinin düşünce hatası | İndirim yüzdesini ondalığa çevirmeyi unutma |
| **Fault** | Kodda oluşan kusur | `total * discountPercent` (100'e bölme yok) |
| **Failure** | Çalışma zamanında yanlış çıktı | %10 indirimde 90 TL yerine -900 TL dönmesi |
| **Defect/Bug** | Raporlanmış hata kaydı | BUG-004: İndirim hesaplama hatası — Kritik |

---

## 📈 Test Stratejileri

| Strateji | Projede Uygulanışı |
|----------|--------------------|
| **Agile Testing** | Her sınıf geliştirilirken eş zamanlı test yazımı, kısa iterasyonlar |
| **Risk-Based Testing** | Yüksek riskli alanlara (stok, ödeme) daha fazla test yoğunlaştırılması |
| **Regression Testing** | Tüm 36 test otomatize; bug fix sonrası tek komutla regression kontrolü |

---

## 📊 Detaylı Rapor

Tüm test case'lerin detaylı tablosu, STLC sürecinin açıklaması, hata kavramları örnekleri ve bug listesi için:

👉 **[REPORT.md](REPORT.md)**

---

## 📜 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.
