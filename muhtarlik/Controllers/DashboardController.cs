

using Microsoft.AspNetCore.Mvc;

namespace muhtarlik.Controllers;


public class DashBoardController : Controller{
     

     private readonly ApplicationDbContext _context;

     public DashBoardController(ApplicationDbContext context)
    {
        _context = context;
    }


    public IActionResult Index()
    {
        // Session'dan kullanıcı bilgilerini al
        var vatandasId = HttpContext.Session.GetString("VatandasId");

        if (vatandasId == null)
        {
            // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
            return RedirectToAction("Login", "Login");
        }

        // Kullanıcı bilgilerini veritabanından al
        var vatandas = _context.Vatandaslar
            .FirstOrDefault(v => v.Id.ToString() == vatandasId);

        if (vatandas == null)
        {
            // Kullanıcı bulunamazsa logout yap ve login sayfasına yönlendir
            return RedirectToAction("Logout", "Login");
        }

        // Kullanıcıya ait bilgileri Dashboard sayfasına gönder
        return View(vatandas);
    }
}