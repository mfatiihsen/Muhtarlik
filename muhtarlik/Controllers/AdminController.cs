using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using muhtarlik.Models;

namespace muhtarlik.Controllers;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
        ViewData["Layout"] = "_AdminLayout";
    }

    public IActionResult Home()
    {
        if (HttpContext.Session.GetString("MuhtarId") == null)
        {
            return RedirectToAction("Login", "Admin");
        }
        else
        {
            var muhtars = _context.Muhtars.ToList();
            return View(muhtars);
        }
    }

    public IActionResult Login()
    {
        return View();
    }

    // Giriş işlemi (POST)
    [HttpPost]
    public async Task<IActionResult> Login(string eposta, string sifre)
    {
        // Boş giriş kontrolü
        if (string.IsNullOrEmpty(eposta) || string.IsNullOrEmpty(sifre))
        {
            ModelState.AddModelError(string.Empty, "Lütfen Email ve Şifre giriniz.");
            return View();
        }

        // Email ve Şifreyi veritabanında kontrol et
        var muhtar = await _context.Muhtars.FirstOrDefaultAsync(m =>
            m.Email == eposta && m.Sifre == sifre
        );

        if (muhtar == null)
        {
            // Eğer kullanıcı bulunamazsa hata mesajı ver
            ModelState.AddModelError(string.Empty, "e-Mail adresiniz veya Şifreniz Yanlış");
            return View();
        }

        if (muhtar.Sifre != sifre)
        {
            // Eğer şifre yanlışsa hata mesajı ekle
            ModelState.AddModelError(string.Empty, "Geçersiz Şifre.");
            return View();
        }

        // Başarılı giriş sonrası session'a MuhtarId'yi kaydet
        HttpContext.Session.SetString("MuhtarId", muhtar.Id.ToString());

        // Muhtar paneline yönlendir
        return RedirectToAction("VatandasListele", "Admin");
    }

    // Çıkış İşlemi Yapmak için yazılan metot
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Admin");
    }

    [HttpGet]
    public IActionResult VatandasListele()
    {
        var vatandaslar = _context.Vatandaslar.ToList();
        return View(vatandaslar);
    }

    [HttpGet]
    public IActionResult VatandasEkle()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> VatandasEkle(VatandasEkleViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var mevcut = await _context.Vatandaslar.FirstOrDefaultAsync(v =>
            v.TcKimlikNo == model.TcKimlikNo
        );

        if (mevcut != null)
        {
            ModelState.AddModelError("TcKimlikNo", "Bu TC Kimlik numarası zaten kayıtlı.");
            return View(model);
        }

        var vatandas = new Vatandas
        {
            TcKimlikNo = model.TcKimlikNo,
            Sifre = model.Sifre,
            Ad = model.Ad,
            Soyad = model.Soyad,
            Cinsiyet = model.Cinsiyet,
            DogumTarihi = model.DogumTarihi,
            Iletisimler = new List<Iletisim>
            {
                new Iletisim { Telefon = model.Telefon, Email = model.Email },
            },
            Adresler = new List<Adres>
            {
                new Adres
                {
                    Il = model.Il,
                    Ilce = model.Ilce,
                    Sokak = model.Sokak,
                    Cadde = model.Cadde,
                    No = model.No,
                    Daire = model.Daire,
                    TamAdres = model.TamAdres,
                },
            },
        };

        _context.Vatandaslar.Add(vatandas);
        await _context.SaveChangesAsync();

        return RedirectToAction("VatandasListele");
    }

    public IActionResult AdresListele()
    {
        var adresler = _context.Adresler.ToList();
        return View(adresler);
    }

    // İletişim Sayfanın Açılması
    [HttpGet]
    public IActionResult IletisimListele()
    {
        var iletisimler = _context.Iletisimler.ToList();
        return View(iletisimler);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult IletisimDuzenle(Iletisim iletisim)
    {
        Console.WriteLine("Gelen ID: " + iletisim.Id);
        Console.WriteLine("Telefon: " + iletisim.Telefon);
        Console.WriteLine("Email: " + iletisim.Email);

        if (ModelState.IsValid)
        {
            // İletişim kaydını ve ilişkili Vatandaş'ı yükle
            var existing = _context
                .Iletisimler.Include(i => i.Vatandas) // İletişim ile ilişkili Vatandaş'ı yükle
                .FirstOrDefault(x => x.Id == iletisim.Id);

            // Eğer bulunduyse, güncelleme işlemi
            if (existing != null)
            {
                // Değerleri güncelle
                existing.Telefon = iletisim.Telefon;
                existing.Email = iletisim.Email;

                // Eğer Vatandaş bilgilerini güncellemek istiyorsanız:
                // existing.Vatandas.Ad = ... (bu satırı kullanarak Vatandaş'ı güncelleyebilirsiniz)

                _context.Entry(existing).State = EntityState.Modified; // Değişiklik olarak işaretle
                _context.SaveChanges();

                TempData["basarili"] = "İletişim bilgileri başarıyla güncellendi.";
                return RedirectToAction("Home");
            }
            else
            {
                return NotFound(); // Kayıt bulunamazsa
            }
        }

        return RedirectToAction("IletisimListele");
    }

    [HttpGet]
    public IActionResult Duyuru()
    {
        var duyurular = _context.Duyurular.ToList();
        return View(duyurular);
    }

    [HttpPost]
    public IActionResult DuyuruEkle(Duyuru duyuru)
    {
        if (ModelState.IsValid)
        {
            duyuru.Tarih = DateTime.Now; // Duyuru eklerken tarih otomatik olarak şu anki zaman alınır
            _context.Duyurular.Add(duyuru);
            _context.SaveChanges();
            return RedirectToAction("Duyuru"); // Listeleme sayfasına yönlendir
        }

        return View(duyuru);
    }

    [HttpPost]
    public IActionResult DuyuruSil(int id)
    {
        var duyuru = _context.Duyurular.Find(id);
        if (duyuru != null)
        {
            _context.Duyurular.Remove(duyuru);
            _context.SaveChanges();
        }
        return RedirectToAction("Duyuru");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DuyuruDuzenle(Duyuru duyuru)
    {
        if (ModelState.IsValid)
        {
            var existingDuyuru = _context.Duyurular.FirstOrDefault(d => d.Id == duyuru.Id);
            if (existingDuyuru != null)
            {
                existingDuyuru.Baslik = duyuru.Baslik;
                existingDuyuru.Icerik = duyuru.Icerik;
                existingDuyuru.Tarih = DateTime.Now; // Tarihi güncelleme

                _context.SaveChanges();

                TempData["basarili"] = "Duyuru başarıyla güncellendi.";
                return RedirectToAction("Duyuru");
            }
            else
            {
                return NotFound();
            }
        }
        return View(duyuru);
    }

    [HttpGet]
    public IActionResult GetAddress(int id)
    {
        var address = _context.Adresler.FirstOrDefault(a => a.Id == id);
        if (address != null)
        {
            return Json(address); // JSON olarak veriyi döndürüyoruz
        }
        return Json(new { success = false, message = "Adres bulunamadı!" });
    }

    [HttpPost]
    public IActionResult AdresDuzenleme(Adres adres)
    {
        if (ModelState.IsValid)
        {
            var existingAddress = _context.Adresler.FirstOrDefault(a => a.Id == adres.Id);
            if (existingAddress != null)
            {
                existingAddress.Il = adres.Il;
                existingAddress.Ilce = adres.Ilce;
                existingAddress.Sokak = adres.Sokak;
                existingAddress.Cadde = adres.Cadde;
                existingAddress.No = adres.No;
                existingAddress.Daire = adres.Daire;
                existingAddress.TamAdres = adres.TamAdres;

                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        return Json(new { success = false });
    }

    [HttpGet]
    public IActionResult Dilekce()
    {
        var dilekceler = _context.Dilekceler.ToList();
        return View(dilekceler);
    }

    [HttpGet]
    public IActionResult Yoneticiler()
    {
        var yoneticiler = _context.Muhtars.ToList();
        return View(yoneticiler);
    }

    [HttpPost]
    public async Task<IActionResult> YoneticiEkle(
        string AdSoyad,
        string Email,
        string Telefon,
        string TcKimlikNo,
        string Sifre
    )
    {
        if (ModelState.IsValid)
        {
            // Şifreyi hash'leyelim
            var passwordHasher = new PasswordHasher<Muhtar>();
            var hashedPassword = passwordHasher.HashPassword(null, Sifre);

            var yeniYonetici = new Muhtar
            {
                AdSoyad = AdSoyad,
                Email = Email,
                Telefon = Telefon,
                TcKimlikNo = TcKimlikNo,
                OlusturmaTarih = DateTime.Now,
                Sifre = hashedPassword, // Şifreyi hash'leyip kaydediyoruz
            };

            _context.Muhtars.Add(yeniYonetici);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Yoneticiler)); // Yöneticiler sayfasına geri dönebilirsiniz
        }

        return View(); // Hata durumunda formu tekrar göstermek için
    }

    // Silme İşlemleri
    [HttpPost]
    public async Task<IActionResult> IletisimSil(int id)
    {
        var iletisim = await _context.Iletisimler.FindAsync(id);

        if (iletisim == null)
        {
            return NotFound();
        }

        _context.Iletisimler.Remove(iletisim);
        await _context.SaveChangesAsync();

        return RedirectToAction("IletisimListele"); // ya da liste sayfasına yönlendirme
    }

    [HttpPost]
    public async Task<IActionResult> AdresSil(int id)
    {
        var adres = await _context.Adresler.FindAsync(id);

        if (adres == null)
        {
            return NotFound();
        }
        _context.Adresler.Remove(adres);
        await _context.SaveChangesAsync();
        return RedirectToAction("AdresListele");
    }

    [HttpPost]
    public async Task<IActionResult> VatandasSil(int id)
    {
        var vatandas = await _context.Vatandaslar.FindAsync(id);

        if (vatandas == null)
        {
            return NotFound();
        }

        _context.Vatandaslar.Remove(vatandas);
        await _context.SaveChangesAsync();
        return RedirectToAction("VatandasListele");
    }

    [HttpPost]
    public async Task<IActionResult> DilekceSil(int id)
    {
        var dilekce = await _context.Dilekceler.FindAsync(id);

        if (dilekce == null)
        {
            return NotFound();
        }
        _context.Dilekceler.Remove(dilekce);
        await _context.SaveChangesAsync();
        return RedirectToAction("Dilekce");
    }

    [HttpPost]
    public async Task<IActionResult> YoneticiSil(int id)
    {
        var yonetici = await _context.Muhtars.FindAsync(id);
        if (yonetici == null)
        {
            return NotFound();
        }

        _context.Muhtars.Remove(yonetici);
        await _context.SaveChangesAsync();
        return RedirectToAction("Yoneticiler");
    }
}
