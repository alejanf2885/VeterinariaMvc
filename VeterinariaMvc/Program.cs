using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using VeterinariaMvc.Repositories.UsuarioRepository;
using VeterinariaMvc.Services.UsuarioService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Inyecciones de dependecias



// Repositories

builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();

//


// Servicios

builder.Services.AddTransient<IUsuarioService, UsuarioService>();


//


// Controladores



//

// Conexion BBDD 
string connectionString =
    builder.Configuration.GetConnectionString("SqlVeterinaria");
builder.Services.AddDbContext<Context>
    (options => options.UseSqlServer(connectionString));


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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
