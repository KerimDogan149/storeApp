🛒 StoreApp - ASP.NET Core MVC E-Ticaret Uygulaması

StoreApp, modern bir ASP.NET Core MVC tabanlı e-ticaret web uygulamasıdır. Hem kullanıcılar için alışveriş deneyimi hem de yöneticiler için kapsamlı bir yönetim paneli sunar.




👤 Kullanıcı Paneli Özellikleri

🛍️ Ürünleri kategori bazlı görüntüleme

🔍 Filtreleme ve arama özellikleri

❤️ Favorilere ürün ekleme / çıkarma

🛒 Sepete ürün ekleme, silme ve sepet güncelleme

📥 Sipariş oluşturma (adres seçimi + ödeme sonrası)

📦 Sipariş geçmişi görüntüleme ve filtreleme

🧾 Sipariş detaylarını inceleme

🏠 Adres ekleme, düzenleme ve silme işlemleri

🗂️ Kişisel kullanıcı paneliyle tam etkileşimli alışveriş deneyimi




🛠️ Yönetici Paneli Özellikleri

🔧 Ürün ve Kategori Yönetimi

Kategori ekleme, silme ve düzenleme

Ürün ekleme, güncelleme, silme işlemleri

Ürün-kategori eşlemesi

Ürün detaylarında "öne çıkan" ve "çok satan" gibi özel etiketler

📥 Excel ile Toplu Ürün İşlemleri

Excel (.xlsx) şablonu ile çoklu ürün yükleme

Var olan ürünlerin Excel üzerinden güncellenmesi

Ön izleme ekranı ile yükleme öncesi kontrol ve onaylama

Başarısız satırlar için detaylı hata mesajları

📦 Stok Yönetimi

Tüm ürünlerin anlık stok görüntüleme tablosu

Stok miktarı güncelleme (manuel artış/azalış)

Stokları kategori ve ürün adı filtreleme ile yönetme

📬 Sipariş Yönetimi

Tüm siparişleri filtreli listeleme

Sipariş detayı görüntüleme

Sipariş durumlarını değiştirme (Hazırlanıyor, Kargoda, Teslim Edildi, İptal, İade)

İade taleplerini inceleme ve onay/red işlemleri (iade süreci opsiyonel olarak eklenecektir)

📊 Raporlama

Tarihe göre satış raporları (günlük, haftalık, aylık)

En çok satılan ürünler listesi

Tüm siparişlerin toplam satış verileri

Grafik veya tablo olarak çıktı alma imkanı (manuel)

🎯 Site Yönetimi

Dinamik slayt yönetimi (başlık, açıklama, görsel ve görünürlük)

Kampanya yönetimi (başlık, açıklama, görsel, aktiflik durumu)

Footer adres ve sosyal medya bağlantılarının kontrolü

👥 Kullanıcı Yönetimi

Kayıtlı kullanıcıları listeleme

Her kullanıcının adresleri ve sipariş geçmişine erişim

Admin kullanıcıları belirleyebilme (geliştirilebilir)


🗂️ Proje Yapısı
StoreApp
│
├── StoreApp.Web
│   ├── Controllers             # Kullanıcı ve admin controller'ları
│   ├── Views                   # Razor View dosyaları (Kullanıcı & Admin)
│   ├── Models                  # ViewModel'ler
│   ├── Components              # Reusable ViewComponent'lar
│   ├── Areas/Admin             # Admin alanına özel controller/view/model
│   ├── Helpers                 # UI yardımcı metotlar ve extensions
│   └── wwwroot                # Statik dosyalar (CSS, JS, img)
│
├── StoreApp.Data
│   ├── StoreDbContext.cs       # EF Core DB Context
│   ├── DataSeed.cs             # Başlangıç verileri
│   ├── Abstract                # Repository interface tanımları
│   ├── Concrete                # EF Core repository implementasyonları
│   ├── Entities                # Entity sınıfları (Product, Order, AppUser, ...)
│   ├── Location                # İl, ilçe, mahalle sınıfları
│   └── Helpers                 # Enum'lar, badge renderer gibi yardımcılar


🧰 Kullanılan Teknolojiler

ASP.NET Core MVC (.NET 9)

Uygulamanın temel web çatısı. Modern, hızlı ve güvenli sunucu taraflı render desteğiyle geliştirilmiştir.

Entity Framework Core 9

ORM (Object-Relational Mapper) ile veri erişimi. Code First yaklaşımla veritabanı işlemleri yapılır.

ASP.NET Identity

Kimlik doğrulama ve kullanıcı yönetimi (giriş, kayıt, roller).

SQLite

Geliştirme aşamasında hızlı ve taşınabilir veritabanı altyapısı.

Bootstrap 5

UI için responsive tasarım desteği.

jQuery + AJAX

Sayfa yenilemeden bazı işlemleri gerçekleştirmek için.

Razor View Engine

Sunucu taraflı HTML render sistemi.

ViewComponent

Modüler ve tekrar kullanılabilir UI bileşenleri.

ExcelDataReader

Excel'den veri okuma için kullanılan kütüphane (toplu ürün işlemleri için).

LINQ & Lambda Expressions

Veri sorgularını daha okunabilir ve güçlü hale getirmek için.

Repository Pattern

Veri erişimini soyutlayarak bağımlılıkları azaltmak için kullanılan tasarım deseni.

Areas

Kullanıcı ve Admin bölümlerini mantıksal olarak ayırmak için.

TagHelpers

Razor view’lerde form ve UI öğeleriyle model binding entegrasyonu için.

Authorization Policies

Sayfa bazlı rol yönetimi.

DataAnnotations

Model doğrulama işlemleri için.

Partial Views

Ortak layout ve bileşenlerde kod tekrarını azaltmak için.



🚀 Başlarken
Projeyi yerel ortamınızda çalıştırmak için aşağıdaki adımları takip edin.
1. Gerekli Paketlerin Yüklenmesi
# EF Core ve diğer bağımlılıkları yükleyin:
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.7
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 9.0.7
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.7
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.7
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 9.0.7
dotnet add package Microsoft.AspNetCore.Hosting.Abstractions --version 2.2.0
2. Veritabanını Oluşturma
cd storeapp.web

# Migration oluştur
dotnet ef migrations add InitialCreate --project ../StoreApp.Data --startup-project .

# Veritabanını güncelle
dotnet ef database update --project ../StoreApp.Data --startup-project .
dotnet watch run


🔐 Varsayılan Yönetici Hesabı
E-posta: admin@storeapp.com

Şifre: Admin123!

Giriş yaptıktan sonra DataSeed.cs ve StoreDbContext.cs dosyalarından varsayılan tanımlı verileri değiştirebilirsiniz.

