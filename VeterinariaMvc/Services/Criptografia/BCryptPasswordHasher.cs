using BCrypt.Net;

namespace VeterinariaMvc.Services.Criptografia
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string HashearPassword(string password)
        {

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return passwordHash;

        }

        public bool VerificarPassword(string password, string hashGuardado)
        {
            bool verificacion = BCrypt.Net.BCrypt.Verify(password,hashGuardado);
            return verificacion;
        }
    }
}
