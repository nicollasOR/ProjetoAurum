using System.Security.Cryptography;
using System.Text;

namespace MVC_AurumLab.Services
{
    public static class DescriptografarHash
    {
        public static byte[] GerarHashBytes(string senha)
        {
            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(senha));
            }
        }
    }
}