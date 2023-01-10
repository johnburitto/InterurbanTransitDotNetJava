using Flight.API.Dtos.Encrypting;

namespace Flight.API.Services.Encrypted
{
    public interface IEncryptedService
    {
        Task<DataTransfer> Encrypt(string text);
    }
}
