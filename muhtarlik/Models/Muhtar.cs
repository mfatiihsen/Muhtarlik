using System.ComponentModel.DataAnnotations;

public class Muhtar
{
    public int Id { get; set; } // Benzersiz ID

    [Required]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 haneli olmalıdır.")]
    public string TcKimlikNo { get; set; } // TC Kimlik No (muhtarın giriş yapacağı ana veri)

    [Required]
    [StringLength(100, ErrorMessage = "Şifre en az 4 karakter olmalıdır.")]
    public string Sifre { get; set; } // Muhtarın şifresi

    [Required]
    public string Telefon { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string OlusturmaTarih { get; set; }
}
