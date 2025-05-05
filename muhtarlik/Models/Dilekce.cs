using System.ComponentModel.DataAnnotations;

namespace muhtarlik.Models;

public class Dilekce
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ad Soyad alanı boş bırakılamaz.")]
    public string AdSoyad { get; set; }

    [Required(ErrorMessage = "TC Kimlik Numarası zorunludur.")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik Numarası 11 haneli olmalıdır.")]
    public string TCKimlikNo { get; set; }

    [Required(ErrorMessage = "Başlık zorunludur.")]
    public string Baslik { get; set; }

    [Required(ErrorMessage = "İçerik boş bırakılamaz.")]
    public string Icerik { get; set; }

    [Required(ErrorMessage = "Telefon numarası giriniz.")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    public string Telefon { get; set; }

    [Required]
    public DateTime Tarih { get; set; } = DateTime.Now; // 🔥 OTOMATİK TARİH
}
