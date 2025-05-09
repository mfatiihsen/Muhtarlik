using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using muhtarlik.Models;

namespace muhtarlik.Controllers;

public class DashBoardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashBoardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var vatandasId = HttpContext.Session.GetString("VatandasId");

        if (vatandasId == null)
        {
            return RedirectToAction("Login", "Login");
        }

        var vatandas = _context.Vatandaslar.FirstOrDefault(v => v.Id.ToString() == vatandasId);

        if (vatandas == null)
        {
            return RedirectToAction("Logout", "Login");
        }

        return View(vatandas);
    }

    [HttpGet]
    public IActionResult Duyurular()
    {
        var duyurular = _context.Duyurular.ToList();
        return View(duyurular);
    }

    [HttpGet]
    public IActionResult Dilekce()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Dilekce(Dilekce model)
    {
        // Kullanıcıdan gelmeyecek alanları ModelState'den çıkar
        ModelState.Remove("AdSoyad");
        ModelState.Remove("TCKimlikNo");
        ModelState.Remove("Telefon");

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Kullanıcı bilgilerini oturumdan çek (örnek)
        model.AdSoyad =
            HttpContext.Session.GetString("Ad") + " " + HttpContext.Session.GetString("Soyad");
        model.TCKimlikNo = HttpContext.Session.GetString("TcKimlikNo");
        model.Telefon = HttpContext.Session.GetString("Telefon");
        model.Tarih = DateTime.Now;

        _context.Dilekceler.Add(model);
        _context.SaveChanges();

        TempData["basarili"] = "Dilekçeniz başarıyla gönderildi.";
        return RedirectToAction("Dilekce");
    }

    public async Task<IActionResult> IletisimGuncelle()
    {
        var vatandasIdStr = HttpContext.Session.GetString("VatandasId");
        if (string.IsNullOrEmpty(vatandasIdStr))
        {
            Console.WriteLine("VatandasId oturumda yok!");
            return RedirectToAction("Login", "Account");
        }
        if (string.IsNullOrEmpty(vatandasIdStr))
        {
            return RedirectToAction("Login", "Account");
        }

        var vatandasId = int.Parse(vatandasIdStr);
        Console.WriteLine("VatandasId: " + vatandasId);
        var vatandas = await _context
            .Vatandaslar.Include(v => v.Iletisimler)
            .FirstOrDefaultAsync(v => v.Id == vatandasId);

        if (vatandas == null)
        {
            return NotFound("Vatandaş bulunamadı.");
        }

        var iletisim = vatandas.Iletisimler.FirstOrDefault();

        if (iletisim == null)
        {
            return NotFound("İletişim bilgisi bulunamadı.");
        }
        return View(iletisim);
    }

    [HttpPost]
    public IActionResult IletisimGuncelle(Iletisim model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var vatandasId = HttpContext.Session.GetString("VatandasId");
        if (vatandasId == null)
        {
            return RedirectToAction("Login", "Vatandas");
        }

        var iletisim = _context.Iletisimler.FirstOrDefault(i =>
            i.VatandasId.ToString() == vatandasId
        );
        if (iletisim == null)
        {
            TempData["Error"] = "İletişim bilgisi bulunamadı.";
            return RedirectToAction("Index");
        }

        iletisim.Telefon = model.Telefon;
        iletisim.Email = model.Email;
        _context.SaveChanges();

        TempData["Success"] = "İletişim bilgileriniz güncellendi.";
        return RedirectToAction("Index");
    }
}
