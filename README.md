# 🚀 Doğuş Teknoloji Blog Uygulaması

**Doğuş Teknoloji Blog**, kullanıcıların teknoloji ve yazılım geliştirme konularında blog yazıları oluşturabileceği, düzenleyebileceği ve yorum yapabileceği bir web uygulamasıdır.

---

## 🎯 Özellikler

### 👤 Kullanıcılar
- Kayıt olabilir, giriş yapabilir ve profil bilgilerini görüntüleyebilir.
- Blog yazıları oluşturabilir, düzenleyebilir ve silebilir.
- Blog yazılarına yorum yapabilir, yorumlarını düzenleyebilir ve silebilir.

### 📝 Blog Yazıları
- Kategorilere göre filtrelenebilir.
- Yorumlar listelenebilir ve sayfa üzerinden eklenebilir.

### 🛠️ Yönetim Paneli
- Kategoriler oluşturulabilir, düzenlenebilir ve silinebilir.

### 🔒 Güvenlik
- CSRF koruması (_AntiForgeryToken_) ile güvenli form işlemleri.
- Yalnızca yetkili kullanıcılar kendi içeriklerini düzenleyebilir veya silebilir.

---

## 🛠️ Kullanılan Teknolojiler

- **ASP.NET Core 9.0** – Web uygulaması geliştirme.
- **Entity Framework Core** – Veritabanı işlemleri için ORM.
- **AutoMapper** – Nesne dönüşümleri için.
- **Bootstrap 5** – Kullanıcı arayüzü tasarımı.
- **jQuery** – Dinamik işlemler ve AJAX istekleri.
- **Microsoft Identity** – Kullanıcı kimlik doğrulama ve yetkilendirme.

---

## 🧱 Mimariler ve Design Pattern'ler

### 🏗️ Katmanlı Mimari (Layered Architecture)

- **Core Katmanı**: Temel iş kuralları ve Entity tanımları.
- **Infrastructure Katmanı**: Veritabanı işlemleri ve Repository implementasyonları.
- **Service Katmanı**: İş mantığı ve DTO yönetimi.
- **Presentation Katmanı**: Razor Pages ve MVC Controller'lar.

### 💡 Design Pattern'ler

- **Repository Pattern**  
  Veritabanı işlemlerini soyutlamak için kullanıldı.  
  Örnek: `PostRepository`, `CommentRepository`

- **Unit of Work Pattern**  
  Birden fazla veritabanı işlemini tek bir işlemde birleştirmek için kullanıldı.  
  Örnek: `IUnitOfWork`, `UnitOfWork`

- **DTO (Data Transfer Object)**  
  Entity'ler ile UI arasında veri taşımak için kullanıldı.  
  Örnek: `PostCreateDto`, `CommentUpdateDto`

- **AutoMapper**  
  DTO'lar ve Entity'ler arasında dönüşüm sağlamak için kullanıldı.  
  Örnek: `PostProfile`, `CategoryProfile`

---

