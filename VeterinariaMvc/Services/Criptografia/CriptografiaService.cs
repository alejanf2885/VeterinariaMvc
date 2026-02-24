namespace VeterinariaMvc.Services.Criptografia
{
    public class CriptografiaService : IPasswordHasher
    {
        public string HashearPassword(string password)
        {
            throw new NotImplementedException();
        }

        public bool VerificarPassword(string password, string hashGuardado)
        {
            if(password == hashGuardado)
            {
                return true;
            }
            return false;
        }
    }
}
