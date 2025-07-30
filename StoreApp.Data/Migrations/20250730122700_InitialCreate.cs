using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StoreApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    SubTitle = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Link = table.Column<string>(type: "TEXT", nullable: true),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsFeatured = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsBestSeller = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteSocialAddressSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    FacebookUrl = table.Column<string>(type: "TEXT", nullable: false),
                    InstagramUrl = table.Column<string>(type: "TEXT", nullable: false),
                    TwitterUrl = table.Column<string>(type: "TEXT", nullable: false),
                    YoutubeUrl = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSocialAddressSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Slides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Link = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slides", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Province = table.Column<string>(type: "TEXT", nullable: false),
                    District = table.Column<string>(type: "TEXT", nullable: false),
                    Neighborhood = table.Column<string>(type: "TEXT", nullable: false),
                    FullAddress = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    AppUserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => new { x.ProductId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ProductCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AppUserId = table.Column<string>(type: "TEXT", nullable: false),
                    AddressId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Campaigns",
                columns: new[] { "Id", "Description", "Image", "IsActive", "Link", "SubTitle", "Title", "Url" },
                values: new object[,]
                {
                    { 1, "İhtiyacını şimdi al, 6 taksitle öde!", "kampanya1.jpg", true, "/kampanya/1", "Son Gün 21 Temmuz", "Peşin Fiyata 6 Taksit", "kampanya-1" },
                    { 2, "Tüm ürünlerde kargo ücretsiz!", "kampanya2.jpg", true, "/kampanya/2", "Her üründe geçerli", "Ücretsiz Kargo", "kampanya-2" },
                    { 3, "İninal Visa ile 1.500 TL’ye varan indirim!", "kampanya3.png", true, "/kampanya/3", "Ekstra indirim fırsatı", "1.500 TL İndirim", "kampanya-3" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Image", "Name", "Url" },
                values: new object[,]
                {
                    { 1, "telefon.jpg", "Telefon", "telefon" },
                    { 2, "laptop.jpg", "Laptop", "laptop" },
                    { 3, "televizyon.jpg", "Televizyon", "televizyon" },
                    { 4, "oyun_konsolu.jpg", "Oyun Konsolu", "oyun_konsolu" },
                    { 5, "tablet.jpg", "Tablet", "tablet" },
                    { 6, "akilli_saat.jpg", "Akıllı Saat", "akilli_saat" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Image", "IsBestSeller", "IsFeatured", "Name", "Price", "Url" },
                values: new object[,]
                {
                    { 1, "iPhone 16, Apple'ın en yeni nesil akıllı telefonudur. Pembe renk seçeneğiyle dikkat çeker. Gelişmiş yapay zeka destekli kamera sistemi, ultra retina XDR ekran, A18 çipi ve uzun pil ömrüyle günlük kullanımı lüks hale getirir.", "iphone16.jpg", false, false, "iPhone 16", 62000m, "iphone-16" },
                    { 2, "A16 Bionic çip ile güçlendirilmiş iPhone 15, enerji verimliliği ve performansı bir araya getiriyor. Kamera özellikleri, sinematik mod ve gelişmiş HDR desteği ile üst düzey çekimler yapmanıza olanak sağlar. Pembe renk ile şık bir görünüm sunar.", "iphone15.jpg", false, false, "iPhone 15", 55000m, "iphone-15" },
                    { 3, "iPhone 14, dayanıklı Ceramic Shield camı ve gelişmiş pil ömrüyle günlük kullanıma tam uyumludur. Mor rengi ve çift arka kamera sistemi ile estetik ve işlevselliği bir araya getirir. Crash Detection gibi hayat kurtaran özellikleriyle donatılmıştır.", "iphone14.jpg", false, false, "iPhone 14", 43000m, "iphone-14" },
                    { 4, "OLED ekran, güçlü A15 Bionic çip ve uzun pil ömrüyle iPhone 13, günlük kullanıcılar için mükemmel bir seçimdir. Beyaz renk seçeneği sade ve zarif bir görünüm sunar. Video ve fotoğraf çekimlerinde Smart HDR 4 teknolojisiyle üst düzey kalite sağlar.", "iphone13.jpg", false, false, "iPhone 13", 33000m, "iphone-13" },
                    { 5, "iPhone 12, 5G teknolojisi ve Super Retina XDR ekranı ile dikkat çeker. Mor renk ile özgün tarzını ortaya koymak isteyenler için ideal. Güçlü performansı ve MagSafe aksesuar desteği ile modern bir deneyim sunar.", "iphone12.jpg", false, false, "iPhone 12", 25000m, "iphone-12" },
                    { 6, "Geniş açı kamerası, dayanıklı cam yapısı ve akıcı iOS deneyimi ile iPhone 11 kullanıcı dostudur. Beyaz rengi ile sade ve modern bir tasarım sunar. Uzun pil ömrü ve iOS güncellemeleri ile hala güçlü bir tercihtir.", "iphone11.jpg", false, false, "iPhone 11", 20000m, "iphone-11" },
                    { 7, "Galaxy S24, Snapdragon 8 Gen 3 işlemcisi ve Dynamic AMOLED 2X ekranıyla üst düzey Android performansı sunar. Kamera sisteminde gece modu, 8K video ve yapay zeka destekli portre çekimleri dikkat çeker. Şık tasarımıyla göz doldurur.", "samsungs24.jpg", false, false, "Samsung Galaxy S24", 44000m, "samsung-galaxy-s24" },
                    { 8, "Samsung Galaxy S23, kompakt tasarımı ve güçlü donanımıyla günlük kullanıma ideal. 120Hz ekran yenileme hızı ve HDR10+ desteğiyle multimedya deneyimini üst seviyeye taşır. Gelişmiş kamera sistemi ile sosyal medya için mükemmel içerikler üretin.", "samsungs23.jpg", false, false, "Samsung Galaxy S23", 38000m, "samsung-galaxy-s23" },
                    { 9, "Galaxy S22, şık tasarımı ve Fiyat/performans dengesiyle öne çıkan bir modeldir. Exynos işlemci, yüksek parlaklığa sahip AMOLED ekran ve kompakt yapısı ile kullanıcı dostu bir Android deneyimi sunar. Uygun fiyatla premium his verir.", "samsungs22.jpg", false, false, "Samsung Galaxy S22", 30000m, "samsung-galaxy-s22" },
                    { 10, "Dell XPS 15, ultra ince tasarımı, 3.5K OLED ekranı ve 13. nesil Intel işlemcisiyle üst düzey performansı bir araya getiriyor. Hem profesyonel işler hem de multimedya için ideal bir dizüstü bilgisayar.", "dellxps15.jpg", false, false, "Dell XPS 15", 115000m, "dell-xps-15" },
                    { 11, "Lenovo Legion Pro, güçlü ekran kartı ve yüksek tazeleme hızına sahip ekranıyla oyuncular için geliştirilmiş bir dizüstü bilgisayardır. RGB klavyesi ve üstün soğutma sistemi ile fark yaratır.", "lenovolegionpro.jpg", false, false, "Lenovo Legion Pro", 95000m, "lenovo-legion-pro" },
                    { 12, "Apple’ın yeni nesil M4 işlemcisine sahip MacBook Air, fan gerektirmeyen sessiz tasarımı ve uzun pil ömrü ile günlük kullanımda mükemmel performans sunar. Ultra hafif yapısıyla taşınabilirlikte lider.", "applemacbookairm4.jpg", false, false, "MacBook Air M4", 130000m, "apple-macbook-air-m4" },
                    { 13, "M3 çipli MacBook Air, Apple ekosistemiyle kusursuz uyum içinde çalışır. Öğrenciler, içerik üreticileri ve günlük kullanıcılar için yüksek performans ve batarya verimliliği sunar.", "applemacbookairm3.jpg", false, false, "MacBook Air M3", 118000m, "apple-macbook-air-m3" },
                    { 14, "Apple’ın ikonik tasarımıyla birleşen M2 işlemcisi sayesinde MacBook Air M2, verimli işlem gücü ve üstün ekran kalitesi ile tanınır. Profesyoneller için ideal bir seçenek.", "applemacbookairm2.jpg", false, false, "MacBook Air M2", 99000m, "apple-macbook-air-m2" },
                    { 15, "MSI Katana GF66, yüksek performanslı NVIDIA RTX ekran kartı, hızlı SSD depolama ve şık tasarımıyla oyun tutkunları için özel olarak üretilmiş bir dizüstü bilgisayardır.", "msikatana.jpg", false, false, "MSI Katana GF66", 87000m, "msi-katana-gf66" },
                    { 16, "HP Victus 16, oyun ve günlük kullanım için dengeli performans sunar. AMD ve Intel varyantlarıyla farklı ihtiyaçlara hitap ederken tasarımıyla da dikkat çeker.", "hpvictus16.jpg", false, false, "HP Victus 16", 78000m, "hp-victus-16" },
                    { 17, "Acer Nitro V5, geniş ekranı, yüksek performanslı donanımı ve etkileyici soğutma sistemiyle özellikle oyuncular ve güç kullanıcıları için geliştirilmiş bir laptop modelidir.", "acernitrov5.jpg", false, false, "Acer Nitro V5", 74000m, "acer-nitro-v5" },
                    { 18, "Samsung'un QLED teknolojisi ile donatılmış bu 55 inçlik televizyon, kristal netliğinde görüntü kalitesi ve HDR desteği ile ev sinema keyfini bir üst seviyeye taşıyor. Akıllı TV özellikleri, yerleşik uygulamalar ve sesli kontrol desteği ile zengin bir kullanıcı deneyimi sunar.", "samsungqledsmart4k.jpg", false, false, "Samsung QLED Smart 4K 55", 32500m, "samsung-qled-smart-4k-55" },
                    { 19, "TCL'in yüksek çözünürlüklü 50 inç Android TV’si, geniş ekran deneyimi, Google TV entegrasyonu ve Dolby Vision teknolojisi ile uygun fiyatlı bir premium seçenek sunar. Minimalist tasarımıyla her odaya uyum sağlar.", "tcl50.jpg", false, false, "TCL 50 4K Ultra HD Android TV", 18900m, "tcl-50-4k-ultra-hd-android-tv" },
                    { 20, "Philips Ambilight özelliği ile ortam aydınlatmasını ekranla senkronize eden bu televizyon, sinema keyfini eve taşıyor. 4K çözünürlük, Dolby Atmos desteği ve Android işletim sistemiyle etkileyici bir performans sunar.", "Philipsambilight.jpg", false, false, "Philips Ambilight 50 Smart TV", 27900m, "philips-ambilight-50-smart-tv" },
                    { 21, "Sony Bravia XR teknolojisi ile görüntüleri yapay zeka desteğiyle işleyerek olağanüstü kontrast ve renk doğruluğu sunar. OLED paneli, derin siyahlar ve etkileyici detaylarla sinema keyfini yeniden tanımlar. Google TV ile zengin uygulama desteği de cabası.", "sonybravia.jpg", false, false, "Sony Bravia XR OLED 55", 48500m, "sony-bravia-xr-oled-55" },
                    { 22, "LG'nin OLED Evo teknolojisiyle donatılmış bu model, olağanüstü görüntü kalitesi ve canlı renklerle dikkat çeker. WebOS işletim sistemi, Magic Remote ve Dolby Vision IQ ile akıllı deneyimi zenginleştirir.", "oledevo4.jpg", false, false, "OLED Evo 4K LG 55", 44900m, "oled-evo-4k-lg-55" },
                    { 23, "Nintendo Switch OLED modeli, canlı renkler ve gelişmiş taşınabilirlik sunar. 7 inç OLED ekran, daha geniş ayarlanabilir stand, gelişmiş ses deneyimi ve 64 GB dahili depolama ile oyun keyfini yeniden tanımlıyor.", "nintendoswitcholed.jpg", false, false, "Nintendo Switch OLED", 13500m, "nintendo-switch-oled" },
                    { 24, "Microsoft'un en güçlü konsolu olan Xbox Series X, 1TB SSD, 4K çözünürlükte 120 FPS oyun desteği ve ultra hızlı yükleme süreleri ile üst düzey performans sunar.", "xboxseriesx.jpg", false, false, "Xbox Series X", 22000m, "xbox-series-x" },
                    { 25, "PlayStation 5 Slim, yüksek performans ve daha ince tasarımı bir araya getiriyor. 4K 120Hz desteği, gelişmiş DualSense kontrolleri ve geniş oyun kütüphanesi ile sizi oyun dünyasına taşır.", "ps5slim.jpg", false, false, "PlayStation 5 Slim", 30000m, "playstation-5-slim" },
                    { 26, "PlayStation Portal, PS5 konsolunuzdaki oyunları uzaktan oynamanızı sağlayan taşınabilir bir cihazdır. 8 inç LCD ekranı ve DualSense entegre kontrolleri ile mobil oyun deneyimi sunar.", "playstationportal.jpg", false, false, "PlayStation Portal", 15000m, "playstation-portal" },
                    { 27, "PlayStation 4 Slim, kompakt tasarımı ve güçlü performansı ile oyun deneyimini ekonomik şekilde yaşamak isteyenler için ideal bir konsoldur.", "playstation4.jpg", false, false, "PlayStation 4 Slim", 21000m, "playstation-4-slim" },
                    { 28, "120 Hz ekran yenileme hızı, Snapdragon işlemcisi ve hafif tasarımıyla yüksek performanslı bir tablet deneyimi sunar.", "huaweimatpad11.jpg", false, false, "Huawei MatePad 11", 10999m, "huawei-matepad-11" },
                    { 29, "Geniş 12.7 inç ekranı ve yüksek çözünürlüğü ile film izleme ve çizim yapma keyfini doruklara çıkarır.", "lenovotab12pro.jpg", false, false, "Lenovo Tab 12 Pro", 12999m, "lenovo-tab-12-pro" },
                    { 30, "2880x1800 çözünürlüklü ekranı, Snapdragon 870 işlemcisi ve Dolby Atmos destekli hoparlörleriyle multimedya için ideal.", "xiaomipad6.jpg", false, false, "Xiaomi Pad 6", 11499m, "xiaomi-pad-6" },
                    { 31, "14.6 inç Dynamic AMOLED ekran, IP68 suya dayanıklılık ve S Pen desteği ile profesyonel seviyede bir Android tablet.", "samsunggalaxytabs9.jpg", false, false, "Samsung Galaxy Tab S9 Ultra", 32999m, "samsung-galaxy-tab-s9-ultra" },
                    { 32, "M4 çipi, Liquid Retina XDR ekranı ve Apple Pencil Pro desteği ile içerik üreticileri ve profesyoneller için en güçlü iPad.", "appleipadpro.jpg", false, false, "Apple iPad Pro", 42000m, "apple-ipad-pro" },
                    { 33, "Ultra ince tasarımı ve gelişmiş AMOLED ekranı ile dikkat çeken Amazfit GTS 4, sağlık takibi ve spor aktiviteleri için ideal.", "amazfitgt6.jpg", false, false, "Amazfit GTS 4", 4799m, "amazfit-gts-4" },
                    { 34, "1.43 inç AMOLED ekranı, 117 spor modu ve uzun pil ömrü ile Xiaomi Watch S1 Active günlük kullanım için mükemmel bir akıllı saat.", "xiaomi-watch-s1-active-siyah.jpg", false, false, "Xiaomi Watch S1 Active", 3899m, "xiaomi-watch-s1-active" },
                    { 35, "Şık tasarımı ve uzun pil ömrüyle dikkat çeken Huawei Watch GT 4, sağlık takibi ve spor özelliklerini bir arada sunar.", "huaweiwatchgt6.jpg", false, false, "Huawei Watch GT 4", 5999m, "huawei-watch-gt-4" },
                    { 36, "Samsung’un son nesil akıllı saati Watch 6, güçlü donanımı ve kapsamlı sağlık özellikleriyle öne çıkıyor.", "samsunggalaxywatch6.jpg", false, false, "Samsung Galaxy Watch 6", 7799m, "samsung-galaxy-watch-6" },
                    { 37, "Apple’ın en yeni akıllı saati Series 9, güçlü işlemcisi ve gelişmiş sağlık özellikleriyle iPhone kullanıcıları için vazgeçilmez.", "applewatchseries.jpg", false, false, "Apple Watch Series 9", 17499m, "apple-watch-series-9" }
                });

            migrationBuilder.InsertData(
                table: "SiteSocialAddressSettings",
                columns: new[] { "Id", "FacebookUrl", "InstagramUrl", "PhoneNumber", "TwitterUrl", "YoutubeUrl" },
                values: new object[] { 1, "https://facebook.com/yourpage", "https://instagram.com/yourpage", "0212 212 12 12", "https://twitter.com/yourpage", "https://youtube.com/yourpage" });

            migrationBuilder.InsertData(
                table: "Slides",
                columns: new[] { "Id", "ImageUrl", "IsActive", "Link", "Order", "Title" },
                values: new object[,]
                {
                    { 1, "banner1.jpg", true, "/kampanya1", 1, "Kampanya 1" },
                    { 2, "banner2.jpg", true, "/kampanya2", 2, "Kampanya 2" },
                    { 3, "banner3.jpg", true, "/kampanya3", 3, "Kampanya 3" }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 1, 5 },
                    { 1, 6 },
                    { 1, 7 },
                    { 1, 8 },
                    { 1, 9 },
                    { 2, 10 },
                    { 2, 11 },
                    { 2, 12 },
                    { 2, 13 },
                    { 2, 14 },
                    { 2, 15 },
                    { 2, 16 },
                    { 2, 17 },
                    { 3, 18 },
                    { 3, 19 },
                    { 3, 20 },
                    { 3, 21 },
                    { 3, 22 },
                    { 4, 23 },
                    { 4, 24 },
                    { 4, 25 },
                    { 4, 26 },
                    { 4, 27 },
                    { 5, 28 },
                    { 5, 29 },
                    { 5, 30 },
                    { 5, 31 },
                    { 5, 32 },
                    { 6, 33 },
                    { 6, 34 },
                    { 6, 35 },
                    { 6, 36 },
                    { 6, 37 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AppUserId",
                table: "Addresses",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressId",
                table: "Orders",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AppUserId",
                table: "Orders",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CategoryId",
                table: "ProductCategories",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "SiteSocialAddressSettings");

            migrationBuilder.DropTable(
                name: "Slides");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
