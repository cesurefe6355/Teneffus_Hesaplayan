<!-- ═══════════════════════════════════════════════════════════════ -->
<!--                        HEADER / BAŞLIK                         -->
<!-- ═══════════════════════════════════════════════════════════════ -->

<div align="center">

<!-- Animasyonlu Başlık (isteğe bağlı) -->
<img src="https://readme-typing-svg.demolab.com?font=Fira+Code&weight=700&size=34&duration=3000&pause=800&color=89B4FA&center=true&vCenter=true&width=700&lines=Okul+Ders+ve+Teneff%C3%BCs+Sayac%C4%B1;%C5%9E%C4%B1k+Tasar%C4%B1m+%7C+Ger%C3%A7ek+Zamanl%C4%B1+Takip;Dinamik+Ada+%7C+Yaprak+Saat+%7C+Karanl%C4%B1k%2FAyd%C4%B1nl%C4%B1k+Tema" alt="Okul Sayacı" />

# 🏫 Okul Ders ve Teneffüs Sayacı

**Modern, animasyonlu ve tamamen özelleştirilebilir bir okul ders programı takip uygulaması.**

Derslerin, teneffüslerin ve öğle arasının ne zaman başlayıp biteceğini **gerçek zamanlı** olarak gösterir. Sadece bir sayaç değil — **3 farklı pencere modu**, **flip clock animasyonu**, **dinamik ada** ve **tema desteğiyle** tam bir masaüstü deneyimi sunar.

<!-- ═══════════════════════════════════════════════════════════════ -->
<!--                            BADGES                              -->
<!-- ═══════════════════════════════════════════════════════════════ -->

[![.NET](https://img.shields.io/badge/.NET-6.0%20%7C%207.0%20%7C%208.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![WPF](https://img.shields.io/badge/WPF-Windows-0078D4?style=for-the-badge&logo=windows&logoColor=white)](https://learn.microsoft.com/dotnet/desktop/wpf/)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![Platform](https://img.shields.io/badge/Platform-Windows%2010%20%7C%2011-0078D6?style=for-the-badge&logo=windows&logoColor=white)](https://www.microsoft.com/windows)

[![License](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](LICENSE)
[![Stars](https://img.shields.io/github/stars/KULLANICI_ADIN/REPO_ADIN?style=for-the-badge&color=F9E2AF&logo=github)](https://github.com/KULLANICI_ADIN/REPO_ADIN/stargazers)
[![Forks](https://img.shields.io/github/forks/KULLANICI_ADIN/REPO_ADIN?style=for-the-badge&color=89B4FA)](https://github.com/KULLANICI_ADIN/REPO_ADIN/network/members)
[![Issues](https://img.shields.io/github/issues/KULLANICI_ADIN/REPO_ADIN?style=for-the-badge&color=F38BA8)](https://github.com/KULLANICI_ADIN/REPO_ADIN/issues)

</div>

---

<!-- ═══════════════════════════════════════════════════════════════ -->
<!--                       ANIMASYONLU ÖNİZLEME                     -->
<!-- ═══════════════════════════════════════════════════════════════ -->

<div align="center">

## 🎬 Önizleme

<!-- Buraya kendi GIF'ini koy. GIF hazırlamak için ScreenToGif (ücretsiz) kullanabilirsin -->
<img src="docs/preview.gif" alt="Uygulama Önizlemesi" width="720"/>

*Sayaç sekmesi · Ders programı · Dinamik ada · Yaprak saat*

</div>

---

## 📖 İçindekiler

- [✨ Özellikler](#-özellikler)
- [🎨 Ekran Görüntüleri](#-ekran-görüntüleri)
- [🚀 Kurulum](#-kurulum)
- [🎮 Kullanım](#-kullanım)
- [🛠️ Teknoloji Yığını](#️-teknoloji-yığını)
- [📂 Proje Yapısı](#-proje-yapısı)
- [⚙️ Yapılandırma](#️-yapılandırma)
- [🗺️ Yol Haritası](#️-yol-haritası)
- [🤝 Katkıda Bulunma](#-katkıda-bulunma)
- [📜 Lisans](#-lisans)

---

## ✨ Özellikler

<table>
<tr>
<td width="50%" valign="top">

### ⏱️ Gerçek Zamanlı Sayaç
- Dairesel ilerleme halkası
- Kalan süreyi `mm:ss` formatında gösterir
- Aktif ders/teneffüs adı ve bitiş saati
- **Son 60 saniye uyarısı** (renk değişir)
- Sıradaki etkinliği otomatik gösterir

</td>
<td width="50%" valign="top">

### 📅 Ders Programı Yönetimi
- Sınırsız ders / teneffüs ekleme
- Satır içi düzenlenebilir DataGrid
- **Exe yanına otomatik kayıt** (`ders_programi.txt`)
- Elle düzenlenebilir `.txt` formatı
- Tek tuşla varsayılan program

</td>
</tr>
<tr>
<td width="50%" valign="top">

### 🏝️ Dinamik Ada Modu
- Ekranın üst ortasına yapışır
- Sadece **310×42 px** — masaüstünü kapatmaz
- **Görev çubuğunda görünmez**
- **Alt+Tab listesinde görünmez**
- Kalan süre + durum + ilerleme çubuğu
- **Her zaman üstte** (topmost)
- Çift tıkla → normal moda dönüş

</td>
<td width="50%" valign="top">

### 🍃 Yaprak Saat Modu
- Gerçek **flip clock** animasyonu
- Açık / karanlık tema uyumlu
- **Sürüklenebilir** pencere
- **9 noktalı akıllı snap** (3×3 grid)
- Bounce animasyonu ile köşeye yapışma
- Hover büyüme efekti

</td>
</tr>
<tr>
<td width="50%" valign="top">

### 🎨 Tema Sistemi
- 🌙 **Karanlık tema** (Catppuccin Mocha)
- ☀️ **Açık tema** (Catppuccin Latte)
- Anında geçiş — tüm pencereler tema uyumlu
- Dinamik ada ve yaprak saat dahil
- Tüm renkler `DynamicResource` ile bağlı

</td>
<td width="50%" valign="top">

### 🚀 Sistem Entegrasyonu
- **Windows ile başlangıçta aç** (Registry)
- Yönetici olarak yeniden başlatma desteği
- Özel toast bildirimleri
- Modern çıkış onayı paneli
- Yazma izni yoksa zarif hata yönetimi

</td>
</tr>
</table>

---

## 🎨 Ekran Görüntüleri

<div align="center">

### 🌙 Karanlık Tema — Sayaç Sekmesi
<img src="docs/dark-counter.png" width="500"/>

### ☀️ Açık Tema — Sayaç Sekmesi
<img src="docs/light-counter.png" width="500"/>

### 📅 Ders Programı Yönetimi
<img src="docs/schedule.png" width="500"/>

### 🏝️ Dinamik Ada Modu
<img src="docs/dynamic-island.png" width="700"/>

### 🍃 Yaprak Saat / Flip Clock
<img src="docs/leaf-clock.gif" width="400"/>

</div>

---

## 🚀 Kurulum

### 📋 Gereksinimler

| Gereksinim | Minimum | Önerilen |
|---|---|---|
| **İşletim Sistemi** | Windows 10 (1809) | Windows 11 |
| **.NET Sürümü** | .NET 6.0 | .NET 8.0 |
| **RAM** | 100 MB boş | 200 MB boş |
| **Disk** | 5 MB | 10 MB |

### ⬇️ Seçenek 1 — Hazır EXE (Önerilen)

1. [**Releases**](../../releases) sayfasına git
2. En son sürümdeki `OkulSayaci.zip` dosyasını indir
3. Zip'i bir klasöre çıkart (örn: `C:\OkulSayaci\`)
4. `OkulSayaci.exe` dosyasına çift tıkla

> 💡 **Not:** Uygulamayı `Program Files` altına kurmak yerine kendi klasörüne koy — böylece ders programı kaydetme izni sorunsuz çalışır.

### 🛠️ Seçenek 2 — Kaynak Koddan Derleme

```bash
# Repoyu klonla
git clone https://github.com/KULLANICI_ADIN/REPO_ADIN.git
cd REPO_ADIN

# Visual Studio ile aç
start OkulSayaci.sln

# Ya da terminalden derle
dotnet build -c Release
dotnet run
```

---

## 🎮 Kullanım

### 📌 Temel Akış

```
┌─────────────────────────────────────────────────┐
│  1. Programı aç                                 │
│  2. ⚙ Ayarlar → Başlangıçta Aç: Kapalı/Açık     │
│  3. 📅 Ders Programı → Kendi programını gir     │
│  4. 💾 Kaydet → Exe yanına otomatik kaydedilir  │
│  5. Sekmeler arasında geçiş yaparak kullan      │
└─────────────────────────────────────────────────┘
```

### ⌨️ Klavye ve Fare Kısayolları

| Kısayol | İşlev |
|---|---|
| **Sol tık (başlık)** | Pencereyi sürükle |
| **Çift tık (ada)** | Normal moda dön |
| **Çift tık (yaprak saat)** | Normal moda dön |
| **Alt + F4** | Ada / yaprak modunda onaysız kapat |
| **✕** | Onay panelini aç |

### 🎛️ Mod Karşılaştırması

| Mod | Boyut | Konum | Kullanım Amacı |
|---|---|---|---|
| **Normal** | 500×640 | Ekran ortası | Tam kontrol |
| **Dinamik Ada** | 280×42 | Ekran üstü ortası | Arka planda takip |
| **Yaprak Saat** | 276×108 | Sürüklenebilir (9 nokta) | Masaüstü süsü |

---

## 🛠️ Teknoloji Yığını

<div align="center">

| Katman | Teknoloji |
|---|---|
| **Dil** | ![C#](https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white&style=flat-square) |
| **UI Framework** | ![WPF](https://img.shields.io/badge/WPF-.NET%20Desktop-0078D4?logo=windows&logoColor=white&style=flat-square) |
| **Runtime** | ![.NET](https://img.shields.io/badge/.NET-6%2B-512BD4?logo=dotnet&logoColor=white&style=flat-square) |
| **Pencere API** | ![Win32](https://img.shields.io/badge/Win32%20API-P/Invoke-5C2D91?style=flat-square) |
| **Kayıt Defteri** | ![Registry](https://img.shields.io/badge/Windows-Registry-0078D4?style=flat-square) |

</div>

### 🎨 Tasarım Sistemi

Uygulama **Catppuccin** renk paletini kullanır:

```
🌙 Karanlık Tema (Mocha)          ☀️ Açık Tema (Latte)
┌──────────────────────┐          ┌──────────────────────┐
│ Arka plan   #181825  │          │ Arka plan   #EFF1F5  │
│ Kart        #1E1E2E  │          │ Kart        #FFFFFF  │
│ Yüzey       #313244  │          │ Yüzey       #E4E8F0  │
│ Metin       #CDD6F4  │          │ Metin       #4C4F69  │
│ Vurgu       #89B4FA  │          │ Vurgu       #1E66F5  │
│ Uyarı       #F9E2AF  │          │ Uyarı       #DF8E1D  │
│ Tehlike     #F38BA8  │          │ Tehlike     #D20F39  │
└──────────────────────┘          └──────────────────────┘
```

---

## 📂 Proje Yapısı

```
OkulSayaci/
│
├── 📁 Tenefus_hesaplayan/
│   ├── 📄 App.xaml                 # Uygulama kaynakları
│   ├── 📄 App.xaml.cs              # Başlangıç noktası
│   ├── 📄 MainWindow.xaml          # Arayüz (XAML)
│   ├── 📄 MainWindow.xaml.cs       # Arka plan kodu (C#)
│   └── 📄 Tenefus_hesaplayan.csproj
│
├── 📁 docs/                        # README görselleri / GIF'ler
│   ├── 🖼️ preview.gif
│   ├── 🖼️ dark-counter.png
│   ├── 🖼️ light-counter.png
│   ├── 🖼️ schedule.png
│   ├── 🖼️ dynamic-island.png
│   └── 🖼️ leaf-clock.gif
│
├── 📄 ders_programi.txt            # Örnek program dosyası (runtime'da oluşur)
├── 📄 README.md                    # Bu dosya
├── 📄 LICENSE                      # MIT Lisansı
└── 📄 .gitignore                   # Git tarafından yok sayılanlar
```

---

## ⚙️ Yapılandırma

### 📝 Ders Programı Dosyası Formatı

Program, exe'nin bulunduğu klasöre `ders_programi.txt` dosyasını kaydeder. İstediğin zaman **not defteri ile elle düzenleyebilirsin**.

```txt
# Tenefüs Hesaplayan - Ders Programı
# Format: EtkinlikAdı|Başlangıç|Bitiş
# Bu dosyayı elle düzenleyebilirsiniz. Program her açılışta okur.
1. Ders|08:30|09:10
1. Teneffüs|09:10|09:20
2. Ders|09:20|10:00
2. Teneffüs|10:00|10:10
...
Öğle Arası|12:30|13:15
...
8. Ders|14:55|15:35
```

**Kurallar:**
- ✅ Her satır `EtkinlikAdı|Başlangıç|Bitiş` formatında olmalı
- ✅ Saat formatı: `HH:mm` (örn: `08:30`, `14:55`)
- ✅ `#` ile başlayan satırlar yorum olarak atlanır
- ✅ Boş satırlar yok sayılır
- ❌ Hatalı satırlar sessizce atlanır, program çökmez

### 🚀 Başlangıçta Otomatik Çalıştırma

Program, başlangıçta çalıştırmak için şu Registry anahtarını kullanır:

```
HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run
    TenefusHesaplayan = "C:\OkulSayaci\OkulSayaci.exe"
```

- 🔓 **Yönetici izni gerektirmez** (kullanıcı bazlı)
- 🗑️ Kapatınca anahtar silinir
- 🔍 Registry Editor'den manuel kontrol edebilirsin

---

## 🗺️ Yol Haritası

- [x] ⏱️ Gerçek zamanlı sayaç
- [x] 🎨 Çift tema desteği
- [x] 🏝️ Dinamik ada modu
- [x] 🍃 Yaprak saat / flip clock
- [x] 💾 Ders programı kaydetme
- [x] 🚀 Başlangıçta otomatik çalışma
- [x] 🛡️ Yönetici izni paneli
- [x] 🎯 9 noktalı akıllı snap
- [ ] 🔔 Ders başlama ses uyarısı
- [ ] 📅 Haftalık / günlük program desteği
- [ ] ☁️ Bulut senkronizasyonu (opsiyonel)
- [ ] 🌍 Çoklu dil desteği (EN/TR)
- [ ] 📊 Aylık istatistik ekranı
- [ ] 🎵 Özel zil sesi yükleme
- [ ] 🖥️ Sistem tepsi (tray) desteği

---

## 🤝 Katkıda Bulunma

Her türlü katkı memnuniyetle karşılanır! 🎉

### 🐛 Hata Bildirimi
1. [Issues](../../issues) sekmesine git
2. **Bug report** şablonunu kullan
3. Hatayı yeniden üretme adımlarını ekle
4. Ekran görüntüsü ekle (varsa)

### 💡 Özellik Önerisi
1. [Issues](../../issues) sekmesine git
2. **Feature request** şablonunu kullan
3. Özelliği neden istediğini açıkla

### 🔧 Pull Request
```bash
# 1. Fork'la
# 2. Feature branch oluştur
git checkout -b feature/yeni-ozellik

# 3. Değişiklikleri commit'le
git commit -m "feat: yeni özellik eklendi"

# 4. Branch'i push'la
git push origin feature/yeni-ozellik

# 5. Pull Request aç
```

### 📝 Commit Kuralları

Bu proje **Conventional Commits** standardını kullanır:

| Prefix | Anlam |
|---|---|
| `feat:` | Yeni özellik |
| `fix:` | Hata düzeltme |
| `docs:` | Dokümantasyon |
| `style:` | Kod stili (davranış değişmez) |
| `refactor:` | Yeniden düzenleme |
| `perf:` | Performans iyileştirmesi |
| `chore:` | Yapılandırma / bağımlılık |

---

## 📜 Lisans

Bu proje **MIT Lisansı** altında dağıtılmaktadır. Detaylar için [LICENSE](LICENSE) dosyasına bakabilirsin.

```
MIT License

Copyright (c) 2025 [Senin Adın]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction...
```

---

## 🙏 Teşekkürler

- 🎨 Renk paleti için [**Catppuccin**](https://github.com/catppuccin/catppuccin)'e
- 💡 İkonlar için [**Lucide**](https://lucide.dev/) ve [**Emoji**](https://emojipedia.org/)'lere
- 📝 README ilhamı için [**Best-README-Template**](https://github.com/othneildrew/Best-README-Template)'e

---

<div align="center">

### ⭐ Bu projeyi beğendiysen yıldız vermeyi unutma!

<img src="https://readme-typing-svg.demolab.com?font=Fira+Code&size=14&duration=4000&pause=1000&color=A6E3A1&center=true&vCenter=true&width=500&lines=Okul+Sayac%C4%B1+ile+dersleri+ka%C3%A7%C4%B1rmak+yok!;%C4%B0yi+kullan%C4%B1mlar+%F0%9F%8E%92" alt="İyi kullanımlar" />

**❤️ ile Türkiye'de yapıldı**

[![GitHub](https://img.shields.io/badge/GitHub-KULLANICI_ADIN-181717?style=for-the-badge&logo=github)](https://github.com/KULLANICI_ADIN)
[![Twitter](https://img.shields.io/badge/Twitter-@kullanici-1DA1F2?style=for-the-badge&logo=twitter&logoColor=white)](https://twitter.com/kullanici)

</div>
