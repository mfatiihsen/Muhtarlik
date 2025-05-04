using System.ComponentModel.DataAnnotations;

public class VatandasEkleViewModel
{
    [Required, StringLength(11, MinimumLength = 11)]
    public string TcKimlikNo { get; set; }

    [Required]
    public string Sifre { get; set; }

    [Required]
    public string Ad { get; set; }

    [Required]
    public string Soyad { get; set; }

    [Required]
    public string Cinsiyet { get; set; }

    [Required]
    public DateTime DogumTarihi { get; set; }

    // İletişim
    [Required]
    public string Telefon { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    // Adres
    [Required]
    public string Il { get; set; }

    public string Ilce { get; set; }
    public string Sokak { get; set; }
    public string Cadde { get; set; }
    public string No { get; set; }
    public string Daire { get; set; }

    public string TamAdres { get; set; }
}
