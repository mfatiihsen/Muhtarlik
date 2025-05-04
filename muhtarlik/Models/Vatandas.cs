

using System.ComponentModel.DataAnnotations;

namespace muhtarlik.Models;

public class Vatandas
{
    [Key] 
    public int Id { get; set; }

    [Required]
    [StringLength(11, MinimumLength = 11)]  // TC Kimlik No uzunluğu 11 karakter olmalı
    public string TcKimlikNo { get; set; }
    
    [Required]
    [StringLength(255)]  // Şifre uzunluğu sınırlı olmalı, genelde 255 yeterlidir
    public string Sifre { get; set; }

    [Required]
    [StringLength(100)]  // Ad uzunluğu maksimum 100 karakter
    public string Ad { get; set; }

    [Required]
    [StringLength(100)]  // Soyad uzunluğu maksimum 100 karakter
    public string Soyad { get; set; }

    [Required]
    public string Cinsiyet { get; set; }

    [Required]
    public DateTime DogumTarihi { get; set; }

    // Navigation properties
    public ICollection<Iletisim> Iletisimler { get; set; }
    public ICollection<Adres> Adresler { get; set; }
    public Basvuru Basvuru { get; set; }
}
