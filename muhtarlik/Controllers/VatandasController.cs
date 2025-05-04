using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        var vatandas = await _context.Vatandaslar.FirstOrDefaultAsync(v =>
            v.TcKimlikNo == tcKimlikNo && v.Sifre == sifre
        );

        if (vatandas == null)
        {
            ModelState.AddModelError(string.Empty, "Geçersiz TC Kimlik No veya Şifre.");
            return View();
        }

        HttpContext.Session.SetString("TcKimlikNo", vatandas.TcKimlikNo);
        HttpContext.Session.SetString("VatandasId", vatandas.Id.ToString());

        return RedirectToAction("Index", "Dashboard");
    }

    // Oturum Kapatma Metodu
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Vatandas");
    }

    [HttpGet]
    public IActionResult SifreDegistir()
    {
        return View();
    }

    // Şifre değiştirme metodu
    [HttpPost]
    public IActionResult SifreDegistir(SifreDegistirViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var tc = HttpContext.Session.GetString("TcKimlikNo");

        if (string.IsNullOrEmpty(tc))
        {
            ModelState.AddModelError(
                "",
                "Oturum süresi dolmuş olabilir. Lütfen tekrar giriş yapın."
            );
            return RedirectToAction("Login", "Vatandas");
        }

        var vatandas = _context.Vatandaslar.FirstOrDefault(v => v.TcKimlikNo == tc);

        if (vatandas == null || vatandas.Sifre != model.EskiSifre)
        {
            ModelState.AddModelError("", "Eski şifre hatalı.");
            model.TcKimlikNo = tc;
            return View(model);
        }

        vatandas.Sifre = model.YeniSifre;
        _context.SaveChanges();

        model.TcKimlikNo = tc;
        Console.WriteLine(tc);
        ViewBag.Mesaj = "Şifre başarıyla değiştirildi.";
        return RedirectToAction("Index", "Dashboard"); // veya başka bir sayfa
    }

}
