# 🐾 VeterinariaMvc

Sistema de gestión clínica veterinaria desarrollado con **ASP.NET Core MVC** y **SQL Server**. Permite administrar usuarios, autenticación, fichas médicas, consultas y archivos multimedia de pacientes animales.

---

## 🚀 Tecnologías

| Tecnología | Versión |
| :--- | :--- |
| .NET | 10.0 |
| ASP.NET Core MVC | 10.0 |
| Entity Framework Core | 10.0.3 |
| SQL Server | - |
| BCrypt.Net-Next | 4.1.0 |
| SixLabors.ImageSharp | 3.1.12 |
| Newtonsoft.Json | 13.0.4 |

---

## 📁 Estructura del Proyecto

```
VeterinariaMvc/
├── Controllers/        # Controladores MVC (Auth, Home)
├── Data/               # Contexto de Entity Framework
├── Dtos/               # Objetos de transferencia de datos
├── Enums/              # Enumeraciones del dominio
├── Extensions/         # Métodos de extensión
├── Helpers/            # Clases auxiliares
├── Mappers/            # Mapeadores entre modelos y DTOs
├── Models/             # Modelos de dominio (entidades)
├── Repositories/       # Capa de acceso a datos
├── Services/           # Lógica de negocio
├── Views/              # Vistas Razor (.cshtml)
├── wwwroot/            # Archivos estáticos (CSS, JS, imágenes)
├── appsettings.json    # Configuración de la aplicación
└── Program.cs          # Punto de entrada y configuración DI
```

---

## ⚙️ Configuración

### Prerrequisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local o remoto)

### Cadena de conexión

Edita el archivo `VeterinariaMvc/appsettings.json` con los datos de tu servidor SQL:

```json
{
  "ConnectionStrings": {
    "SqlVeterinaria": "Data Source=LOCALHOST\\DEVELOPER;Initial Catalog=Veterinaria;Persist Security Info=True;User ID=SA;Password=TU_PASSWORD;Encrypt=True;Trust Server Certificate=True"
  }
}
```

---

## ▶️ Cómo ejecutar

```bash
# Clona el repositorio
git clone https://github.com/alejanf2885/VeterinariaMvc.git
cd VeterinariaMvc

# Ejecuta la aplicación
dotnet run --project VeterinariaMvc/VeterinariaMvc.csproj
```

La aplicación estará disponible en `https://localhost:5001` (o el puerto configurado).

---

## 🏗️ Arquitectura

El proyecto sigue el patrón **MVC** con separación clara de responsabilidades:

- **Controllers** – Reciben peticiones HTTP y coordinan la respuesta.
- **Services** – Contienen la lógica de negocio (autenticación, gestión de imágenes, etc.).
- **Repositories** – Acceden a la base de datos mediante Entity Framework Core.
- **Models** – Representan las entidades de la base de datos.
- **DTOs** – Objetos utilizados para transferir datos entre capas y vistas.

### Servicios principales

| Servicio | Descripción |
| :--- | :--- |
| `IAuthService` | Login y registro de usuarios |
| `IUsuarioService` | Gestión de usuarios |
| `IEstadoUsuarioService` | Manejo de sesión activa |
| `IImagenService` | Procesamiento y almacenamiento de imágenes |
| `IFileStorageService` | Almacenamiento de archivos en disco |
| `IPasswordHasher` | Hash seguro de contraseñas con BCrypt |

---

## 🗄️ Base de Datos

La base de datos se llama **Veterinaria** y utiliza SQL Server. Consulta el archivo [`VeterinariaMvc/README.md`](VeterinariaMvc/README.md) para ver el diseño completo del esquema, el diccionario de datos y el motor de plantillas EAV.

---

## 🔒 Seguridad

- Las contraseñas se almacenan con **BCrypt** (hash + salt).
- La sesión del usuario se gestiona mediante **ASP.NET Core Session** con expiración de 1 hora.
- Las imágenes se procesan y sanean antes de almacenarse para evitar archivos maliciosos.

---

## 📈 Roadmap

1. **Fase 1** ✅ Seguridad, autenticación y gestión de usuarios.
2. **Fase 2** 🔄 Gestión de mascotas y dueños.
3. **Fase 3** ⏳ Consultas, plantillas dinámicas y carga de archivos.
4. **Fase 4** ⏳ Tratamientos, seguimientos y reportes estadísticos.

---

## 🤝 Contribuir

1. Haz un fork del repositorio.
2. Crea una rama con tu funcionalidad: `git checkout -b feature/nueva-funcionalidad`.
3. Realiza tus cambios y haz commit: `git commit -m "feat: descripción del cambio"`.
4. Sube los cambios: `git push origin feature/nueva-funcionalidad`.
5. Abre un Pull Request.
