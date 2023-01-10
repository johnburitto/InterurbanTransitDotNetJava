using Flight.API.Data;
using Flight.API.Dtos.Encrypting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace Flight.API.Services.Encrypted
{
    public class EncryptedService : IEncryptedService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _client;
        private readonly ILogger<EncryptedService> _logger;

        public EncryptedService(AppDbContext context, HttpClient client, ILogger<EncryptedService> logger)
        {
            _context = context;
            _client = client;
            _logger = logger;
        }

        public async Task<DataTransfer> Encrypt(string text)
        {
            var textTransfer = new TextTransfer { Text = text };
            var jsonEntity = JsonConvert.SerializeObject(textTransfer);
            var content = new StringContent(jsonEntity, Encoding.Unicode, "application/json");
            var response = await _client.PostAsync("/encrypt", content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<DataTransfer>(response.Content.ReadAsStringAsync().Result);

                return result ?? throw new ArgumentNullException();
            }

            _logger.LogError("Some error was occured during encrypting");
            throw new Exception();
        }
    }
}
