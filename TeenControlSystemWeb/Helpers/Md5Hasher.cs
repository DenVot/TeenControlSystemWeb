using System.Security.Cryptography;
using System.Text;

namespace TeenControlSystemWeb.Helpers;

public class Md5Hasher : IDisposable
{
    private readonly MD5 _md5;
    
    public Md5Hasher()
    {
        _md5 = MD5.Create();
    }
    
    public string HashString(string toHash)
    {
        var wordBytes = Encoding.UTF8.GetBytes(toHash);
        
        var bytes = _md5.ComputeHash(wordBytes);
        var hash = BitConverter.ToString(bytes)
            .Replace("-",
                string.Empty)
            .ToLower();

        return hash;
    }
    
    public void Dispose()
    {
        _md5.Dispose();
    }
}