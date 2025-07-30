using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using StoreApp.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data.Entities;
namespace StoreApp.Data.Concrete;

public class StoreDbContext : IdentityDbContext<AppUser>
{
    public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();

    public DbSet<Slide> Slides => Set<Slide>();

    public DbSet<Campaign> Campaigns => Set<Campaign>();

    public DbSet<Address> Addresses => Set<Address>();

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();




    public DbSet<SiteSocialAddressSetting> SiteSocialAddressSettings => Set<SiteSocialAddressSetting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
            entity.Property(u => u.PhoneNumber).HasMaxLength(20);

        });

        // Composite Key for ProductCategory
        modelBuilder.Entity<ProductCategory>()
            .HasKey(pc => new { pc.ProductId, pc.CategoryId });


        modelBuilder.Entity<ProductCategory>()
            .HasOne(pc => pc.Product)
            .WithMany(p => p.ProductCategories)
            .HasForeignKey(pc => pc.ProductId);

        modelBuilder.Entity<ProductCategory>()
            .HasOne(pc => pc.Category)
            .WithMany(c => c.ProductCategories)
            .HasForeignKey(pc => pc.CategoryId);

        modelBuilder.Entity<Product>().HasData(
            new List<Product>() {
        new() { Id=1, Name="iPhone 16", Price=62000, Description="iPhone 16, Apple'ın en yeni nesil akıllı telefonudur. Pembe renk seçeneğiyle dikkat çeker. Gelişmiş yapay zeka destekli kamera sistemi, ultra retina XDR ekran, A18 çipi ve uzun pil ömrüyle günlük kullanımı lüks hale getirir.",Image="iphone16.jpg",Url="iphone-16" },
        new() { Id=2, Name="iPhone 15", Price=55000, Description="A16 Bionic çip ile güçlendirilmiş iPhone 15, enerji verimliliği ve performansı bir araya getiriyor. Kamera özellikleri, sinematik mod ve gelişmiş HDR desteği ile üst düzey çekimler yapmanıza olanak sağlar. Pembe renk ile şık bir görünüm sunar.", Image="iphone15.jpg",Url="iphone-15" },
        new() { Id=3, Name="iPhone 14", Price=43000, Description="iPhone 14, dayanıklı Ceramic Shield camı ve gelişmiş pil ömrüyle günlük kullanıma tam uyumludur. Mor rengi ve çift arka kamera sistemi ile estetik ve işlevselliği bir araya getirir. Crash Detection gibi hayat kurtaran özellikleriyle donatılmıştır.", Image="iphone14.jpg",Url="iphone-14" },
        new() { Id=4, Name="iPhone 13", Price=33000, Description="OLED ekran, güçlü A15 Bionic çip ve uzun pil ömrüyle iPhone 13, günlük kullanıcılar için mükemmel bir seçimdir. Beyaz renk seçeneği sade ve zarif bir görünüm sunar. Video ve fotoğraf çekimlerinde Smart HDR 4 teknolojisiyle üst düzey kalite sağlar.", Image="iphone13.jpg",Url="iphone-13" },
        new() { Id=5, Name="iPhone 12", Price=25000, Description="iPhone 12, 5G teknolojisi ve Super Retina XDR ekranı ile dikkat çeker. Mor renk ile özgün tarzını ortaya koymak isteyenler için ideal. Güçlü performansı ve MagSafe aksesuar desteği ile modern bir deneyim sunar.", Image="iphone12.jpg",Url="iphone-12" },
        new() { Id=6, Name="iPhone 11", Price=20000, Description="Geniş açı kamerası, dayanıklı cam yapısı ve akıcı iOS deneyimi ile iPhone 11 kullanıcı dostudur. Beyaz rengi ile sade ve modern bir tasarım sunar. Uzun pil ömrü ve iOS güncellemeleri ile hala güçlü bir tercihtir.", Image="iphone11.jpg",Url="iphone-11" },
        new() { Id=7, Name="Samsung Galaxy S24", Price=44000, Description="Galaxy S24, Snapdragon 8 Gen 3 işlemcisi ve Dynamic AMOLED 2X ekranıyla üst düzey Android performansı sunar. Kamera sisteminde gece modu, 8K video ve yapay zeka destekli portre çekimleri dikkat çeker. Şık tasarımıyla göz doldurur.", Image="samsungs24.jpg",Url="samsung-galaxy-s24" },
        new() { Id=8, Name="Samsung Galaxy S23", Price=38000, Description="Samsung Galaxy S23, kompakt tasarımı ve güçlü donanımıyla günlük kullanıma ideal. 120Hz ekran yenileme hızı ve HDR10+ desteğiyle multimedya deneyimini üst seviyeye taşır. Gelişmiş kamera sistemi ile sosyal medya için mükemmel içerikler üretin.", Image="samsungs23.jpg",Url="samsung-galaxy-s23" },
        new() { Id=9, Name="Samsung Galaxy S22", Price=30000, Description="Galaxy S22, şık tasarımı ve Fiyat/performans dengesiyle öne çıkan bir modeldir. Exynos işlemci, yüksek parlaklığa sahip AMOLED ekran ve kompakt yapısı ile kullanıcı dostu bir Android deneyimi sunar. Uygun fiyatla premium his verir.", Image="samsungs22.jpg",Url="samsung-galaxy-s22" },
        new() { Id=10, Name="Dell XPS 15", Price=115000, Description="Dell XPS 15, ultra ince tasarımı, 3.5K OLED ekranı ve 13. nesil Intel işlemcisiyle üst düzey performansı bir araya getiriyor. Hem profesyonel işler hem de multimedya için ideal bir dizüstü bilgisayar.", Image="dellxps15.jpg",Url="dell-xps-15" },
        new() { Id=11, Name="Lenovo Legion Pro", Price=95000, Description="Lenovo Legion Pro, güçlü ekran kartı ve yüksek tazeleme hızına sahip ekranıyla oyuncular için geliştirilmiş bir dizüstü bilgisayardır. RGB klavyesi ve üstün soğutma sistemi ile fark yaratır.", Image="lenovolegionpro.jpg",Url="lenovo-legion-pro" },
        new() { Id=12, Name="MacBook Air M4", Price=130000, Description="Apple’ın yeni nesil M4 işlemcisine sahip MacBook Air, fan gerektirmeyen sessiz tasarımı ve uzun pil ömrü ile günlük kullanımda mükemmel performans sunar. Ultra hafif yapısıyla taşınabilirlikte lider.", Image="applemacbookairm4.jpg",Url="apple-macbook-air-m4" },
        new() { Id=13, Name="MacBook Air M3", Price=118000, Description="M3 çipli MacBook Air, Apple ekosistemiyle kusursuz uyum içinde çalışır. Öğrenciler, içerik üreticileri ve günlük kullanıcılar için yüksek performans ve batarya verimliliği sunar.",Image="applemacbookairm3.jpg",Url="apple-macbook-air-m3" },
        new() { Id=14, Name="MacBook Air M2", Price=99000, Description="Apple’ın ikonik tasarımıyla birleşen M2 işlemcisi sayesinde MacBook Air M2, verimli işlem gücü ve üstün ekran kalitesi ile tanınır. Profesyoneller için ideal bir seçenek.", Image="applemacbookairm2.jpg",Url="apple-macbook-air-m2" },
        new() { Id=15, Name="MSI Katana GF66", Price=87000, Description="MSI Katana GF66, yüksek performanslı NVIDIA RTX ekran kartı, hızlı SSD depolama ve şık tasarımıyla oyun tutkunları için özel olarak üretilmiş bir dizüstü bilgisayardır.", Image="msikatana.jpg",Url="msi-katana-gf66" },
        new() { Id=16, Name="HP Victus 16", Price=78000, Description="HP Victus 16, oyun ve günlük kullanım için dengeli performans sunar. AMD ve Intel varyantlarıyla farklı ihtiyaçlara hitap ederken tasarımıyla da dikkat çeker.", Image="hpvictus16.jpg",Url="hp-victus-16" },
        new() { Id=17, Name="Acer Nitro V5", Price=74000, Description="Acer Nitro V5, geniş ekranı, yüksek performanslı donanımı ve etkileyici soğutma sistemiyle özellikle oyuncular ve güç kullanıcıları için geliştirilmiş bir laptop modelidir.", Image="acernitrov5.jpg",Url="acer-nitro-v5" },
        new() { Id=18, Name="Samsung QLED Smart 4K 55", Price=32500, Description="Samsung'un QLED teknolojisi ile donatılmış bu 55 inçlik televizyon, kristal netliğinde görüntü kalitesi ve HDR desteği ile ev sinema keyfini bir üst seviyeye taşıyor. Akıllı TV özellikleri, yerleşik uygulamalar ve sesli kontrol desteği ile zengin bir kullanıcı deneyimi sunar.", Image="samsungqledsmart4k.jpg",Url="samsung-qled-smart-4k-55" },
        new() { Id=19, Name="TCL 50 4K Ultra HD Android TV", Price=18900, Description="TCL'in yüksek çözünürlüklü 50 inç Android TV’si, geniş ekran deneyimi, Google TV entegrasyonu ve Dolby Vision teknolojisi ile uygun fiyatlı bir premium seçenek sunar. Minimalist tasarımıyla her odaya uyum sağlar.", Image="tcl50.jpg",Url="tcl-50-4k-ultra-hd-android-tv" },
        new() { Id=20, Name="Philips Ambilight 50 Smart TV", Price=27900, Description="Philips Ambilight özelliği ile ortam aydınlatmasını ekranla senkronize eden bu televizyon, sinema keyfini eve taşıyor. 4K çözünürlük, Dolby Atmos desteği ve Android işletim sistemiyle etkileyici bir performans sunar.", Image="Philipsambilight.jpg",Url="philips-ambilight-50-smart-tv" },
        new() { Id=21, Name="Sony Bravia XR OLED 55", Price=48500, Description="Sony Bravia XR teknolojisi ile görüntüleri yapay zeka desteğiyle işleyerek olağanüstü kontrast ve renk doğruluğu sunar. OLED paneli, derin siyahlar ve etkileyici detaylarla sinema keyfini yeniden tanımlar. Google TV ile zengin uygulama desteği de cabası.", Image="sonybravia.jpg",Url="sony-bravia-xr-oled-55" },
        new() { Id=22, Name="OLED Evo 4K LG 55", Price=44900, Description="LG'nin OLED Evo teknolojisiyle donatılmış bu model, olağanüstü görüntü kalitesi ve canlı renklerle dikkat çeker. WebOS işletim sistemi, Magic Remote ve Dolby Vision IQ ile akıllı deneyimi zenginleştirir.", Image="oledevo4.jpg",Url="oled-evo-4k-lg-55" },
        new() { Id=23, Name="Nintendo Switch OLED", Price=13500, Description="Nintendo Switch OLED modeli, canlı renkler ve gelişmiş taşınabilirlik sunar. 7 inç OLED ekran, daha geniş ayarlanabilir stand, gelişmiş ses deneyimi ve 64 GB dahili depolama ile oyun keyfini yeniden tanımlıyor.", Image="nintendoswitcholed.jpg",Url="nintendo-switch-oled" },
        new() { Id=24, Name="Xbox Series X", Price=22000, Description="Microsoft'un en güçlü konsolu olan Xbox Series X, 1TB SSD, 4K çözünürlükte 120 FPS oyun desteği ve ultra hızlı yükleme süreleri ile üst düzey performans sunar.", Image="xboxseriesx.jpg",Url="xbox-series-x" },
        new() { Id=25, Name="PlayStation 5 Slim", Price=30000, Description="PlayStation 5 Slim, yüksek performans ve daha ince tasarımı bir araya getiriyor. 4K 120Hz desteği, gelişmiş DualSense kontrolleri ve geniş oyun kütüphanesi ile sizi oyun dünyasına taşır.", Image="ps5slim.jpg",Url="playstation-5-slim" },
        new() { Id=26, Name="PlayStation Portal", Price=15000, Description="PlayStation Portal, PS5 konsolunuzdaki oyunları uzaktan oynamanızı sağlayan taşınabilir bir cihazdır. 8 inç LCD ekranı ve DualSense entegre kontrolleri ile mobil oyun deneyimi sunar.", Image="playstationportal.jpg",Url="playstation-portal" },
        new() { Id=27, Name="PlayStation 4 Slim", Price=21000, Description="PlayStation 4 Slim, kompakt tasarımı ve güçlü performansı ile oyun deneyimini ekonomik şekilde yaşamak isteyenler için ideal bir konsoldur.", Image="playstation4.jpg",Url="playstation-4-slim" },
        new() { Id=28, Name="Huawei MatePad 11", Price=10999, Description="120 Hz ekran yenileme hızı, Snapdragon işlemcisi ve hafif tasarımıyla yüksek performanslı bir tablet deneyimi sunar.", Image="huaweimatpad11.jpg",Url="huawei-matepad-11" },
        new() { Id=29, Name="Lenovo Tab 12 Pro", Price=12999, Description="Geniş 12.7 inç ekranı ve yüksek çözünürlüğü ile film izleme ve çizim yapma keyfini doruklara çıkarır.", Image="lenovotab12pro.jpg",Url="lenovo-tab-12-pro" },
        new() { Id=30, Name="Xiaomi Pad 6", Price=11499, Description="2880x1800 çözünürlüklü ekranı, Snapdragon 870 işlemcisi ve Dolby Atmos destekli hoparlörleriyle multimedya için ideal.", Image="xiaomipad6.jpg",Url="xiaomi-pad-6" },
        new() { Id=31, Name="Samsung Galaxy Tab S9 Ultra", Price=32999, Description="14.6 inç Dynamic AMOLED ekran, IP68 suya dayanıklılık ve S Pen desteği ile profesyonel seviyede bir Android tablet.", Image="samsunggalaxytabs9.jpg",Url="samsung-galaxy-tab-s9-ultra"},
        new() { Id=32, Name="Apple iPad Pro", Price=42000, Description="M4 çipi, Liquid Retina XDR ekranı ve Apple Pencil Pro desteği ile içerik üreticileri ve profesyoneller için en güçlü iPad.", Image="appleipadpro.jpg",Url="apple-ipad-pro" },
        new() { Id=33, Name="Amazfit GTS 4", Price=4799, Description="Ultra ince tasarımı ve gelişmiş AMOLED ekranı ile dikkat çeken Amazfit GTS 4, sağlık takibi ve spor aktiviteleri için ideal.", Image="amazfitgt6.jpg",Url="amazfit-gts-4" },
        new() { Id=34, Name="Xiaomi Watch S1 Active", Price=3899, Description="1.43 inç AMOLED ekranı, 117 spor modu ve uzun pil ömrü ile Xiaomi Watch S1 Active günlük kullanım için mükemmel bir akıllı saat.", Image="xiaomi-watch-s1-active-siyah.jpg",Url="xiaomi-watch-s1-active" },
        new() { Id=35, Name="Huawei Watch GT 4", Price=5999, Description="Şık tasarımı ve uzun pil ömrüyle dikkat çeken Huawei Watch GT 4, sağlık takibi ve spor özelliklerini bir arada sunar.", Image="huaweiwatchgt6.jpg",Url="huawei-watch-gt-4"},
        new() { Id=36, Name="Samsung Galaxy Watch 6", Price=7799, Description="Samsung’un son nesil akıllı saati Watch 6, güçlü donanımı ve kapsamlı sağlık özellikleriyle öne çıkıyor.", Image="samsunggalaxywatch6.jpg",Url="samsung-galaxy-watch-6" },
        new() { Id=37, Name="Apple Watch Series 9", Price=17499, Description="Apple’ın en yeni akıllı saati Series 9, güçlü işlemcisi ve gelişmiş sağlık özellikleriyle iPhone kullanıcıları için vazgeçilmez.", Image="applewatchseries.jpg",Url="apple-watch-series-9"}

        }
        );

        modelBuilder.Entity<Category>().HasData(
            new List<Category>() {
                new () { Id = 1,  Name = "Telefon", Url = "telefon",Image = "telefon.jpg"},
                new () { Id = 2,  Name = "Laptop", Url = "laptop", Image = "laptop.jpg"},
                new () { Id = 3,  Name = "Televizyon", Url = "televizyon",Image = "televizyon.jpg"},
                new () { Id = 4,  Name = "Oyun Konsolu", Url = "oyun_konsolu",Image = "oyun_konsolu.jpg"},
                new () { Id = 5,  Name = "Tablet", Url = "tablet",Image = "tablet.jpg"},
                new () { Id = 6,  Name = "Akıllı Saat", Url = "akilli_saat",Image = "akilli_saat.jpg"}

            }
        );

        modelBuilder.Entity<ProductCategory>().HasData(
            new List<ProductCategory>() {
            new() { ProductId = 1, CategoryId = 1 },
            new() { ProductId = 2, CategoryId = 1 },
            new() { ProductId = 3, CategoryId = 1 },
            new() { ProductId = 4, CategoryId = 1 },
            new() { ProductId = 5, CategoryId = 1 },
            new() { ProductId = 6, CategoryId = 1 },
            new() { ProductId = 7, CategoryId = 1 },
            new() { ProductId = 8, CategoryId = 1 },
            new() { ProductId = 9, CategoryId = 1 },
            new() { ProductId = 10, CategoryId = 2 },
            new() { ProductId = 11, CategoryId = 2 },
            new() { ProductId = 12, CategoryId = 2 },
            new() { ProductId = 13, CategoryId = 2 },
            new() { ProductId = 14, CategoryId = 2 },
            new() { ProductId = 15, CategoryId = 2 },
            new() { ProductId = 16, CategoryId = 2 },
            new() { ProductId = 17, CategoryId = 2 },
            new() { ProductId = 18, CategoryId = 3 },
            new() { ProductId = 19, CategoryId = 3 },
            new() { ProductId = 20, CategoryId = 3 },
            new() { ProductId = 21, CategoryId = 3 },
            new() { ProductId = 22, CategoryId = 3 },
            new() { ProductId = 23, CategoryId = 4 },
            new() { ProductId = 24, CategoryId = 4 },
            new() { ProductId = 25, CategoryId = 4 },
            new() { ProductId = 26, CategoryId = 4 },
            new() { ProductId = 27, CategoryId = 4 },
            new() { ProductId = 28, CategoryId = 5 },
            new() { ProductId = 29, CategoryId = 5 },
            new() { ProductId = 30, CategoryId = 5 },
            new() { ProductId = 31, CategoryId = 5 },
            new() { ProductId = 32, CategoryId = 5 },
            new() { ProductId = 33, CategoryId = 6 },
            new() { ProductId = 34, CategoryId = 6 },
            new() { ProductId = 35, CategoryId = 6 },
            new() { ProductId = 36, CategoryId = 6 },
            new() { ProductId = 37, CategoryId = 6 }
            }
        );

        modelBuilder.Entity<Slide>().HasData(new List<Slide>
            {
            new ()
            {
                Id = 1,
                ImageUrl = "banner1.jpg",
                Title = "Kampanya 1",
                Link = "/kampanya1",
                IsActive = true,
                Order = 1
            },
            new ()
            {
                Id = 2,
                ImageUrl = "banner2.jpg",
                Title = "Kampanya 2",
                Link = "/kampanya2",
                IsActive = true,
                Order = 2
            },
            new ()
            {
                Id = 3,
                ImageUrl = "banner3.jpg",
                Title = "Kampanya 3",
                Link = "/kampanya3",
                IsActive = true,
                Order = 3
            }
        });

        modelBuilder.Entity<Campaign>().HasData(new List<Campaign>
        {
            new()
            {
                Id = 1,
                Title = "Peşin Fiyata 6 Taksit",
                SubTitle = "Son Gün 21 Temmuz",
                Description = "İhtiyacını şimdi al, 6 taksitle öde!",
                Image = "kampanya1.jpg",
                Url = "kampanya-1",
                Link = "/kampanya/1"
            },
            new()
            {
                Id = 2,
                Title = "Ücretsiz Kargo",
                SubTitle = "Her üründe geçerli",
                Description = "Tüm ürünlerde kargo ücretsiz!",
                Image = "kampanya2.jpg",
                Url = "kampanya-2",
                Link = "/kampanya/2"
            },
            new()
            {
                Id = 3,
                Title = "1.500 TL İndirim",
                SubTitle = "Ekstra indirim fırsatı",
                Description = "İninal Visa ile 1.500 TL’ye varan indirim!",
                Image = "kampanya3.png",
                Url = "kampanya-3",
                Link = "/kampanya/3"
            }
        });

        modelBuilder.Entity<SiteSocialAddressSetting>().HasData(new SiteSocialAddressSetting
        {
            Id = 1,
            PhoneNumber = "0212 212 12 12",
            FacebookUrl = "https://facebook.com/yourpage",
            InstagramUrl = "https://instagram.com/yourpage",
            TwitterUrl = "https://twitter.com/yourpage",
            YoutubeUrl = "https://youtube.com/yourpage"
        });


    }
}
