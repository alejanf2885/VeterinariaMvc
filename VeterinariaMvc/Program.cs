using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using VeterinariaMvc.Repositories.Auth;
using VeterinariaMvc.Repositories.UsuarioRepository;
using VeterinariaMvc.Services.Auth;
using VeterinariaMvc.Services.Criptografia;
using VeterinariaMvc.Services.Estado;
using VeterinariaMvc.Services.UsuarioService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Inyecciones de dependecias



// Repositories

builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IAuthUsuarioRepository, AuthUsuarioRepository>();

//


// Servicios

builder.Services.AddTransient<IUsuarioService, UsuarioService>();
builder.Services.AddTransient<IPasswordHasher, CriptografiaService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IEstadoUsuarioService, SessionUsuarioService>();




//


// Controladores



//

// Conexion BBDD 
string connectionString =
    builder.Configuration.GetConnectionString("SqlVeterinaria");
builder.Services.AddDbContext<Context>
    (options => options.UseSqlServer(connectionString));



builder.Services.AddHttpContextAccessor();

//Cache
builder.Services.AddDistributedMemoryCache();

//CREAMOS UN SERVICIO DE SESSION
builder.Services.AddSession(options =>
{

    options.IdleTimeout = TimeSpan.FromHours(1);

});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

//HABILITAMOS SESSION
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
