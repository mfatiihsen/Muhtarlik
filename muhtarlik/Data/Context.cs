

using Microsoft.EntityFrameworkCore;
using muhtarlik.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Vatandas> Vatandaslar { get; set; }
    public ISet<Iletisim> Iletisimler { get; set; }
    public DbSet<Adres> Adresler { get; set; }
    public DbSet<Basvuru> Basvurular { get; set; }
    public DbSet<Muhtar> Muhtars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Foreign Key ilişkileri burada da tanımlanabilir
        modelBuilder.Entity<Iletisim>()
            .HasOne(i => i.Vatandas)
            .WithMany(v => v.Iletisimler)
            .HasForeignKey(i => i.VatandasId)
            .IsRequired();

        modelBuilder.Entity<Adres>()
            .HasOne(a => a.Vatandas)
            .WithMany(v => v.Adresler)
            .HasForeignKey(a => a.VatandasId)
            .IsRequired();

        modelBuilder.Entity<Basvuru>()
            .HasOne(b => b.Vatandas)
            .WithOne(v => v.Basvuru)
            .HasForeignKey<Basvuru>(b => b.VatandasId)
            .IsRequired();
    }
}
