namespace TicketingSystem.Core.ServiceContracts;

public interface ICryptoService
{
    string Encrypt(string plainText, out string salt);

    bool Verify(string plainText, string hash, string salt);
}