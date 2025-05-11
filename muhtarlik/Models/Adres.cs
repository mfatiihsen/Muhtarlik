using System.ComponentModel.DataAnnotations;

namespace muhtarlik.Models;

public class Adres
{
    public int Id { get; set; }

    [Required]
    public int VatandasId { get; set; }

    [Required]
    [StringLength(500)] // Adres çok uzun olabilir, ama 500 karakter sınırı yeterli olabilir
    public string Il { get; set; }
    public string Ilce { get; set; }
    public string Sokak { get; set; }
    public string Cadde { get; set; }
    public string No { get; set; }
    public string Daire { get; set; }
    public string TamAdres { get; set; }

    // Navigation property
    public Vatandas? Vatandas { get; set; }
}
