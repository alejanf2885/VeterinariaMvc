using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using VeterinariaMvc.Data;
using VeterinariaMvc.Enums;
using VeterinariaMvc.Models;
using VeterinariaMvc.Models.Enums;
using static Azure.Core.HttpHeader;
using static System.Runtime.InteropServices.JavaScript.JSType;

#region STORED PROCEDURE



//CREATE OR ALTER PROCEDURE SP_REGISTRARUSUARIO
//    @Email nvarchar(200),
//    @Nombre nvarchar(200),
//    @Telefono nvarchar(40),
//    @Imagen nvarchar(510),
//    @IdRol int,
//    @TipoAuth nvarchar(100),
//    @PasswordHash nvarchar(510),
//    @NewID int OUTPUT
//AS
//BEGIN
//    SET NOCOUNT ON;
//BEGIN TRANSACTION
//        -- 1. Insertar el Usuario
//        INSERT INTO USUARIO(EMAIL, NOMBRE, TELEFONO, IMAGEN, ID_ROL, ACTIVO, FECHA_CREACION)
//        VALUES (@Email, @Nombre, @Telefono, @Imagen, @IdRol, 1, GETDATE());

//SET @NewID = SCOPE_IDENTITY(); --Asignar a la variable de salida

//        -- 2. Insertar la Credencial
//        INSERT INTO CREDENCIAL(ID_USUARIO, TIPO, PASSWORD_HASH, FECHA_ACTUALIZACION)
//        VALUES (@NewID, @TipoAuth, @PasswordHash, GETDATE());

//--3: Insertar automáticamente en la tabla CLIENTE
//        INSERT INTO CLIENTE(ID_USUARIO)
//        VALUES (@NewID);

//COMMIT TRANSACTION
//END
//GO

#endregion

namespace VeterinariaMvc.Repositories.UsuarioRepository
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private Context context;

        public UsuarioRepository(Context context)
        {
            this.context = context;
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {

            var consulta = from datos in this.context.Usuarios
                           where datos.Email.Equals(email)
                           select datos;

            bool existe = await consulta.AnyAsync();

            return existe;

        }

        public async Task<Usuario?> ObtenerPorEmailAsync(string email)
        {

            var consulta = from datos in this.context.Usuarios
                           where datos.Email.Equals(email)
                           select datos;

            Usuario usuario = await consulta.FirstOrDefaultAsync();

            return usuario;
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.Id == id
                           select datos;

            Usuario usuario = await consulta.FirstOrDefaultAsync();

            return usuario;

        }

        public async Task<Usuario?> RegistrarUsuarioAsync
          (string email, string nombre, string telefono, string rutaImagen, TipoCredencial tipoAuth, string passwordHash)
        {
            string sql = "EXEC SP_REGISTRARUSUARIO @Email, @Nombre, @Telefono, @Imagen, @IdRol, @TipoAuth, @PasswordHash, @NewID OUTPUT";

            SqlParameter pamEmail = new SqlParameter("@Email", email);
            SqlParameter pamNombre = new SqlParameter("@Nombre", nombre);
            SqlParameter pamTelefono = new SqlParameter("@Telefono", telefono ?? (object)DBNull.Value);
            SqlParameter pamImagen = new SqlParameter("@Imagen", rutaImagen ?? (object)DBNull.Value);
            SqlParameter pamIdRol = new SqlParameter("@IdRol", Roles.Usuario);
            SqlParameter pamTipoAuth = new SqlParameter("@TipoAuth", tipoAuth);
            SqlParameter pamPasswordHash = new SqlParameter("@PasswordHash", passwordHash);

            SqlParameter pamIdOut = new SqlParameter();
            pamIdOut.ParameterName = "@NewID";
            pamIdOut.SqlDbType = SqlDbType.Int;
            pamIdOut.Direction = ParameterDirection.Output;

            // Ejecutamos el procedimiento
            await this.context.Database
                .ExecuteSqlRawAsync(sql, pamEmail, pamNombre, pamTelefono, pamImagen, pamIdRol, pamTipoAuth, pamPasswordHash, pamIdOut);

            int newId = (int)pamIdOut.Value;

            if (newId == -1)
            {
                return null;
            }

            Usuario usuario = await this.ObtenerPorIdAsync(newId);

            return usuario;
        }
    }
}
