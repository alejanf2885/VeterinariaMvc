namespace VeterinariaMvc.Services.Criptografia
{
    public interface IPasswordHasher
    {
        string HashearPassword(string password);
        bool VerificarPassword(string password, string hashGuardado);
    }
}
