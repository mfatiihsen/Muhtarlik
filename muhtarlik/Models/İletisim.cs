using System.ComponentModel.DataAnnotations;

namespace muhtarlik.Models;

public class Iletisim
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string Telefon { get; set; }

    [Required]
    [EmailAddress] // E-posta adresi formatını kontrol eder
    public string Email { get; set; }

    // Navigation property
    public int VatandasId { get; set; }
    public Vatandas? Vatandas { get; set; }
}
