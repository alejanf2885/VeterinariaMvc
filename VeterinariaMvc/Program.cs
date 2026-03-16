using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization; // <-- NUEVO: Para las políticas
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
using VeterinariaMvc.Security; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// ==========================================
// REPOSITORIES
// ==========================================
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

// ==========================================
// SERVICIOS
// ==========================================
builder.Services.AddTransient<IUsuarioService, UsuarioService>();
builder.Services.AddTransient<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddScoped<IEstadoUsuarioService, ClaimsUsuarioService>();

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

// ==========================================
// SEGURIDAD: AUTENTICACIÓN Y AUTORIZACIÓN
// ==========================================
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Home/Error";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

builder.Services.AddScoped<IAuthorizationHandler, PermisoMascotaHandler>();
builder.Services.AddScoped<IAuthorizationHandler, PermisoTratamientoHandler>();
builder.Services.AddScoped<IAuthorizationHandler, PermisoConsultaHandler>();
builder.Services.AddScoped<IAuthorizationHandler, PermisoChatHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PoliticaPermisoMascota", policy =>
    {
        policy.Requirements.Add(new PermisoMascotaRequirement());
    });
    options.AddPolicy("PoliticaPermisoTratamiento", policy =>
    {
        policy.Requirements.Add(new PermisoTratamientoRequirement());
    });
    options.AddPolicy("PoliticaPermisoConsulta", policy =>
    {
        policy.Requirements.Add(new PermisoConsultaRequirement());
    });
    options.AddPolicy("PoliticaPermisoChat", policy =>
    {
        policy.Requirements.Add(new PermisoChatRequirement());
    });
});
// ==========================================

// ==========================================
// CONEXIÓN A BASE DE DATOS
// ==========================================
string connectionString = builder.Configuration.GetConnectionString("SqlVeterinaria");
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));

builder.Services.AddHttpContextAccessor();

// Cache y WebSockets (SignalR para el chat)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSignalR();

// SERVICIO DE SESSION 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
});

var app = builder.Build();

// ==========================================
// MIDDLEWARES DE PIPELINE
// ==========================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.UseSession();

// ==========================================
// ENRUTAMIENTO Y PROTECCIÓN DE ÁREAS
// ==========================================

app.MapAreaControllerRoute(
    name: "AreaCliente",
    areaName: "Cliente",
    pattern: "Cliente/{controller=Home}/{action=Index}/{id?}")
    .RequireAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<ChatHub>("/chatHub");

app.Run();