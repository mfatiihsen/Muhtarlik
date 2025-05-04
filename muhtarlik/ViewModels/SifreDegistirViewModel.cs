using System.ComponentModel.DataAnnotations;

public class SifreDegistirViewModel
{
    public string TcKimlikNo { get; set; }

    [Required(ErrorMessage = "Eski şifre zorunludur")]
    public string EskiSifre { get; set; }

    [Required(ErrorMessage = "Yeni şifre zorunludur")]
    [MinLength(4, ErrorMessage = "Şifre en az 4 karakter olmalı")]
    public string YeniSifre { get; set; }

    [Compare("YeniSifre", ErrorMessage = "Şifreler eşleşmiyor")]
    public string YeniSifreTekrar { get; set; }
}
