# Belediye Talep ve Şikâyet Yönetim Sistemi

Bu proje, vatandaşların belediyeye ait talep ve şikâyetlerini çevrim içi olarak iletebilmesi için geliştirilmiş web tabanlı bir yönetim sistemidir. Sistem üzerinden vatandaşlar talep oluşturabilir, konum ve fotoğraf ekleyebilir, oluşturdukları taleplerin durumunu takip edebilir. Personeller kendi bağlı oldukları müdürlüğe gelen talepleri görüntüleyip durum güncellemesi yapabilir. Yöneticiler ise tüm talepleri, müdürlükleri, dashboard verilerini ve harita üzerindeki şikâyetleri takip edebilir.

## Projenin Amacı

Bu projenin amacı, belediyeye gelen talep ve şikâyetlerin dijital ortamda daha düzenli şekilde kaydedilmesini ve takip edilmesini sağlamaktır. Böylece vatandaş başvuruları kaybolmadan sisteme alınır, ilgili müdürlüklere yönlendirilir ve süreç hem personel hem de yönetici tarafından daha kolay takip edilir.

## Kullanıcı Rolleri

Sistemde üç farklı kullanıcı rolü bulunmaktadır:

### Vatandaş

- Sisteme kayıt olabilir ve giriş yapabilir.
- E-posta doğrulaması yapmadan sisteme giriş yapamaz.
- Yeni talep veya şikâyet oluşturabilir.
- Talebe açık adres, konum ve fotoğraf ekleyebilir.
- Kendi taleplerini listeleyebilir.
- Talep detayını ve durumunu takip edebilir.
- Gerekirse kendi talebini düzenleyebilir veya silebilir.

### Personel

- Sadece kendi bağlı olduğu müdürlüğe ait aktif talepleri görüntüler.
- Talep detaylarını inceleyebilir.
- Talebin durumunu güncelleyebilir.
- Talebi “Tamamlandı” yaptığında sistem onay ister.
- Onay verilirse talep aktif personel listesinden kaldırılır ancak veritabanından silinmez.

### Yönetici

- Sistemdeki tüm talepleri görüntüleyebilir.
- Yönetici panelinden genel talep ve kullanıcı sayılarını görebilir.
- Dashboard ekranından talep dağılımlarını inceleyebilir.
- Harita üzerinden konumlu şikâyetleri takip edebilir.
- Müdürlüklere göre renkli harita noktalarını görüntüleyebilir.
- Müdürlük yönetimi işlemlerini yapabilir.

## Temel Özellikler

- Kullanıcı kayıt ve giriş sistemi
- Rol bazlı yetkilendirme
- E-posta doğrulama sistemi
- Talep ve şikâyet oluşturma
- Talep listeleme, detay görüntüleme, düzenleme ve silme
- Açık adres, haritadan konum seçme ve fotoğraf yükleme
- Leaflet.js ile harita entegrasyonu
- Personel-müdürlük eşleştirme
- Birim bazlı talep görüntüleme
- Talep durum güncelleme
- Tamamlanan talepleri aktif listeden kaldırma
- Yönetici dashboard ekranı
- Müdürlüklere göre renkli şikâyet haritası
- Metin analizi ile kategori, müdürlük ve öncelik önerisi
- Öncelik rozetleri ve önceliğe göre sıralama
- Modernleştirilmiş belediye temalı arayüz

## Kullanılan Teknolojiler

- ASP.NET Core MVC
- C#
- Entity Framework Core
- Microsoft SQL Server
- Bootstrap
- JavaScript
- Leaflet.js
- SMTP / Gmail App Password
- User Secrets
- HTML
- CSS

## Veritabanı Yapısı

Projede Entity Framework Core Code First yaklaşımı kullanılmıştır. Veritabanı Microsoft SQL Server üzerinde oluşturulmuştur.

Başlıca tablolar:

- `ApplicationUsers`
- `Talepler`
- `Mudurlukler`
- `TalepDurumlari`

## E-posta Doğrulama

Kullanıcı kayıt olduğunda sistem otomatik olarak bir doğrulama tokeni oluşturur. Kullanıcı hesabı ilk olarak `EmailConfirmed = false` olarak kaydedilir. Doğrulama bağlantısı kullanıcının e-posta adresine gönderilir. Kullanıcı maildeki bağlantıya tıkladığında token kontrol edilir ve hesap doğrulanır.

Mail şifresi güvenlik nedeniyle `appsettings.json` içinde tutulmamıştır. Bunun yerine `user-secrets` kullanılmıştır.

## Harita Özelliği

Talep oluşturma ekranında kullanıcı harita üzerinden konum seçebilir. Seçilen konum enlem ve boylam olarak veritabanına kaydedilir. Personel, vatandaş ve yönetici ekranlarında bu konum harita üzerinde görüntülenebilir.

Yönetici dashboard ekranında şikâyetler müdürlüklere göre farklı renklerle gösterilir:

- Fen İşleri: Kırmızı
- Temizlik İşleri: Yeşil
- Park ve Bahçeler: Mavi
- Zabıta: Turuncu
- Ulaşım Hizmetleri: Mor
- Diğer: Gri

## Metin Analizi ve Öncelik Sistemi

Talep oluşturma sırasında kullanıcının yazdığı başlık ve açıklama alanları sistem tarafından analiz edilir. Bu analiz sonucunda kategori, müdürlük ve öncelik önerisi yapılır.

Bu yapı kural tabanlı metin analizi mantığıyla çalışmaktadır. Örneğin su baskını, tehlike, kaza gibi ifadeler geçen talepler yüksek öncelikli olarak işaretlenebilir. Yüksek öncelikli talepler listelerde üst sıralarda gösterilir.

