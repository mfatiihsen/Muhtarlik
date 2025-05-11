using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using muhtarlik.Models;

namespace muhtarlik.Controllers;

public class VatandasController : Controller
{
    private readonly ApplicationDbContext _context;

    public VatandasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Giriş Sayfasına Yönlendirme
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // Giriş Yapma metodu
    [HttpPost]
    public async Task<IActionResult> Login(string tcKimlikNo, string sifre)
    {
        if (string.IsNullOrEmpty(tcKimlikNo) || string.IsNullOrEmpty(sifre))
        {
            ModelState.AddModelError(string.Empty, "Lütfen TC Kimlik No ve Şifre giriniz.");
            return View();
        }

        // Vatandas'ı ve ilişkili Iletisim'i bul
        var vatandas = await _context
            .Vatandaslar.Include(v => v.Iletisimler) // İletişim bilgilerini dahil et
            .FirstOrDefaultAsync(v => v.TcKimlikNo == tcKimlikNo && v.Sifre == sifre);

        if (vatandas == null)
        {
            ModelState.AddModelError(string.Empty, "Geçersiz TC Kimlik No veya Şifre.");
            return View();
        }

        // Session'a bilgileri kaydet
        HttpContext.Session.SetString("TcKimlikNo", vatandas.TcKimlikNo);
        HttpContext.Session.SetString("Ad", vatandas.Ad);
        HttpContext.Session.SetString("Soyad", vatandas.Soyad);
        var telefon = vatandas.Iletisimler?.FirstOrDefault()?.Telefon ?? "Telefon bulunamadı";
        HttpContext.Session.SetString("Telefon", telefon); // Telefonu oturuma kaydet

        HttpContext.Session.SetString("VatandasId", vatandas.Id.ToString());

        return RedirectToAction("Index", "Dashboard");
    }

    // Oturum Kapatma Metodu

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Vatandas");
    }

    [HttpGet]
    public IActionResult SifreDegistir()
    {
        var tc = HttpContext.Session.GetString("TcKimlikNo");

        var model = new SifreDegistirViewModel { TcKimlikNo = tc };

        return View(model);
    }

    // Şifre değiştirme metodu
    [HttpPost]
    public IActionResult SifreDegistir(SifreDegistirViewModel model)
    {
        var tc = HttpContext.Session.GetString("TcKimlikNo");
        model.TcKimlikNo = tc; // View tekrar dönerse Tc dolu olsun

        if (string.IsNullOrEmpty(tc))
        {
            // Oturum yoksa login'e yönlendir
            TempData["Error"] = "Oturum süresi dolmuş olabilir. Lütfen tekrar giriş yapın.";
            return RedirectToAction("Login", "Vatandas");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var vatandas = _context.Vatandaslar.FirstOrDefault(v => v.TcKimlikNo == tc);

        if (vatandas == null)
        {
            ModelState.AddModelError("", "Vatandaş bilgisi bulunamadı.");
            return View(model);
        }

        if (vatandas.Sifre != model.EskiSifre)
        {
            ModelState.AddModelError("", "Eski şifre hatalı.");
            return View(model);
        }

        vatandas.Sifre = model.YeniSifre;
        _context.SaveChanges();

        TempData["Success"] = "Şifreniz başarıyla değiştirildi.";
        return RedirectToAction("Index", "Dashboard");
    }
}
