using Microsoft.AspNetCore.Mvc;
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
    public IActionResult Dilekce(Dilekce dilekce)
    {
        if (ModelState.IsValid)
        {
            // Bu satıra aslında gerek kalmaz çünkü modelde default veriyoruz:
            // dilekce.Tarih = DateTime.Now;

            _context.Dilekceler.Add(dilekce);
            _context.SaveChanges();
            TempData["basarili"] = "Dilekçeniz başarıyla gönderildi.";
            return RedirectToAction("Index");
        }

        return View(dilekce);
    }
}
