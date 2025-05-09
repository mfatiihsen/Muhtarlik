using System.ComponentModel.DataAnnotations;

namespace muhtarlik.Models;

public class Muhtar
{
    public int Id { get; set; }

    [Required]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 haneli olmalıdır.")]
    public string TcKimlikNo { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Şifre en az 4 karakter olmalıdır.")]
    public string Sifre { get; set; }

    [Required]
    public string Telefon { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string AdSoyad { get; set; }

    [Required]
    public DateTime OlusturmaTarih { get; set; }
}
