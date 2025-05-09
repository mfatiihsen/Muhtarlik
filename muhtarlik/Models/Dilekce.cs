using System.ComponentModel.DataAnnotations;

namespace muhtarlik.Models;

public class Dilekce
{
    public int Id { get; set; }

    public string AdSoyad { get; set; } // Otomatik eklenecek (Kullanıcıdan alınmayacak)
    public string TCKimlikNo { get; set; } // Otomatik eklenecek
    public string Telefon { get; set; } // Otomatik eklenecek

    [Required(ErrorMessage = "Başlık zorunludur.")]
    public string Baslik { get; set; }

    [Required(ErrorMessage = "İçerik boş bırakılamaz.")]
    public string Icerik { get; set; }

    [Required]
    public DateTime Tarih { get; set; } = DateTime.Now;
}
