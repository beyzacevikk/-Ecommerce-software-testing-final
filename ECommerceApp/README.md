# ECommerceApp - Yazılım Test Final Projesi

## 📌 Proje Hakkında
Bu proje, bir e-ticaret sisteminin temel fonksiyonlarını içermekte ve yazılım test kavramlarının uygulamalı olarak gösterilmesini amaçlamaktadır.

**Özellikler:**
- Ürün yönetimi ve stok kontrolü
- Sepet işlemleri (ekleme, çıkarma, toplam hesaplama)
- Sipariş verme ve ödeme simülasyonu
- İndirim (discount) uygulama
- Minimum sipariş tutarı kontrolü

**Test:** Sistemde bilerek bırakılmış 6 adet bug bulunmaktadır. Bu buglar NUnit testleri ile tespit edilmektedir.

## 🏗️ Proje Yapısı

```
ECommerceApp/
├── Core/
│   ├── Product.cs          # Ürün sınıfı
│   ├── Cart.cs             # Sepet sınıfı
│   └── OrderService.cs     # Sipariş servisi
├── Tests/
│   ├── UnitTests/
│   │   └── ECommerceTests.cs   # Unit + BlackBox + GrayBox testler
│   └── IntegrationTests/
│       └── OrderIntegrationTests.cs  # Integration testler
├── Program.cs              # Demo konsol uygulaması
├── ECommerceApp.csproj     # Proje dosyası
├── REPORT.md               # Test raporu
└── README.md               # Bu dosya
```

## 🔧 Kurulum ve Çalıştırma

### Gereksinimler
- .NET 8.0 SDK

### Projeyi Derleme
```bash
dotnet build
```

### Demo Çalıştırma
```bash
dotnet run
```

### Testleri Çalıştırma
```bash
dotnet test --verbosity normal
```

## 🧪 Test Özeti

| Metrik | Değer |
|--------|-------|
| Toplam Test | 27 |
| Passed | 21 |
| Failed | 6 |
| Başarı Oranı | %77.8 |

### Test Türleri
- **Unit Test (White Box):** 7 test
- **Black Box Test:** 9 test
- **Gray Box Test:** 7 test
- **Integration Test:** 4 test

### Tespit Edilen Buglar
1. **BUG-001:** ReduceStock — `>` yerine `>=` olmalı
2. **BUG-002:** IsValid — Negatif fiyat kontrolü eksik
3. **BUG-003:** PlaceOrder — Max indirim limiti kontrol edilmiyor
4. **BUG-004:** ApplyDiscount — Yüzde hesaplama hatası (100'e bölme yok) [KRİTİK]
5. **BUG-005:** PlaceOrder — Minimum sipariş sınır değer hatası (`<=` → `<`)
6. **BUG-006:** PlaceOrder — BUG-001'in zincirleme etkisi

Detaylı rapor için: [REPORT.md](REPORT.md)

## 📊 Kullanılan Test Teknikleri
- Equivalence Partitioning (EP)
- Boundary Value Analysis (BVA)

## 📈 Test Stratejileri
- Agile Testing
- Risk-Based Testing
- Regression Testing
