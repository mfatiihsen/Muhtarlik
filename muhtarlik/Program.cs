using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Veri tabanı bağlantısı
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MuhtarlikDb"))
);

builder.Services.AddDistributedMemoryCache(); // Bellek tabanlı cache'i kullanıyoruz
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; // Güvenlik için HttpOnly özelliğini etkinleştiriyoruz
    options.Cookie.IsEssential = true; // Cookie'lerin temel özellik olarak kabul edilmesini sağlıyoruz
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session süresi
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession(); // Bu, Session kullanabilmek için gerekli

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=Vatandas}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();
