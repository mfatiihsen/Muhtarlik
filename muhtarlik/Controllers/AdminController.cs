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
        return RedirectToAction("Home", "Admin");
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

    [HttpGet]
    public IActionResult Dilekce()
    {
        var dilekceler = _context.Dilekceler.ToList();
        return View(dilekceler);
    }

    [HttpGet]
    public IActionResult Yoneticiler()
    {
        return View();
    }
}
