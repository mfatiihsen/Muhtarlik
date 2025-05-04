

using System.ComponentModel.DataAnnotations;

namespace muhtarlik.Models;

public class Basvuru
{
    public int Id { get; set; }

    [Required]
    public int VatandasId { get; set; }
    public bool BasvuruDurumu { get; set; }
    public DateTime OlusturulmaTarihi { get; set; }

    // Navigation property
    public Vatandas Vatandas { get; set; }
}
