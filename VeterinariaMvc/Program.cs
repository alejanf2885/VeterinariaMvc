using Microsoft.EntityFrameworkCore;
using VeterinariaMvc.Data;
using VeterinariaMvc.Repositories.Auth;
using VeterinariaMvc.Repositories.Clinica;
using VeterinariaMvc.Repositories.Consulta;
using VeterinariaMvc.Repositories.EspecieRepository;
using VeterinariaMvc.Repositories.MascotasRepository;
using VeterinariaMvc.Repositories.RazaRepository;
using VeterinariaMvc.Repositories.UsuarioRepository;
using VeterinariaMvc.Services.Archivos;
using VeterinariaMvc.Services.Auth;
using VeterinariaMvc.Services.Clinica;
using VeterinariaMvc.Services.Consulta;
using VeterinariaMvc.Services.Criptografia;
using VeterinariaMvc.Services.Estado;
using VeterinariaMvc.Services.FileStorage;
using VeterinariaMvc.Services.Imagenes;
using VeterinariaMvc.Services.MascotaCatalogosService;
using VeterinariaMvc.Services.Mascotas;
using VeterinariaMvc.Services.SaneadorFotos;
using VeterinariaMvc.Services.UsuarioService;
using VeterinariaMvc.Services.Tratamientos;
using VeterinariaMvc.Repositories.Tratamientos;
using VeterinariaMvc.Repositories.Chats;
using VeterinariaMvc.Services.Chats;
using VeterinariaMvc.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Inyecciones de dependecias



// Repositories

builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IAuthUsuarioRepository, AuthUsuarioRepository>();
builder.Services.AddTransient<IMascotasRepository, MascotasRepository>();
builder.Services.AddTransient<IRazaRepository, RazaRepository>();
builder.Services.AddTransient<IEspecieRepository, EspecieRepository>();
builder.Services.AddTransient<IVeterinarioRepository, VeterinarioRepository>();
builder.Services.AddTransient<IClinicaRepository, ClinicaRepository>();
builder.Services.AddTransient<IConsultaRepository, ConsultaRepository>();
builder.Services.AddTransient<ITratamientoRepository, TratamientoRepository>();
builder.Services.AddTransient<IChatRepository, ChatRepository>();

//


// Servicios

builder.Services.AddTransient<IUsuarioService, UsuarioService>();
builder.Services.AddTransient<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IEstadoUsuarioService, SessionUsuarioService>();
builder.Services.AddTransient<INombreArchivoService, NombreArchivoService>();
builder.Services.AddTransient<IProcesadorImagenService, ProcesadorImagenService>();
builder.Services.AddTransient<IFileStorageService, FileStorageService>();
builder.Services.AddTransient<IImagenService, ImagenService>();
builder.Services.AddTransient<IMascotasService, MascotasService>();
builder.Services.AddTransient<IMascotaCatalogoService, MascotaCatalogoService>();
builder.Services.AddTransient<IClinicaService, ClinicaService>();
builder.Services.AddTransient<IConsultaService, ConsultaService>();
builder.Services.AddTransient<ITratamientoService, TratamientoService>();
builder.Services.AddTransient<IChatService, ChatService>();







//


// Conexion BBDD 
string connectionString =
    builder.Configuration.GetConnectionString("SqlVeterinaria");
builder.Services.AddDbContext<Context>
    (options => options.UseSqlServer(connectionString));



builder.Services.AddHttpContextAccessor();

//Cache
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSignalR();

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
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<ChatHub>("/chatHub");

app.Run();
