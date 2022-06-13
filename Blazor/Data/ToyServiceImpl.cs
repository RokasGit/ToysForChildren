using System.Text;
using System.Text.Json;
using Blazor.Models;

namespace Blazor.Data;

public class ToyServiceImpl : IToyService
{
    private string uri = "https://localhost:7096";
    private HttpClient _client;

    public ToyServiceImpl()
    {
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
        {
            return true;
        };
        _client = new HttpClient(clientHandler);
    }
    public async Task AddToyAsync(Toy toy, int childId)
    {
        string toyAsJson = JsonSerializer.Serialize(toy);
        StringContent content = new StringContent(toyAsJson, Encoding.UTF8, "application/json");
        HttpResponseMessage responseMessage = await _client.PostAsync($"{uri}/Toy/owner/{childId}", content);
        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {responseMessage.StatusCode} {responseMessage.ReasonPhrase}");
        }
    }

    public async Task RemoveToy(int toyId)
    {
        try
        {
            HttpResponseMessage responseMessage = await _client.DeleteAsync($"{uri}/Toy/{toyId}");
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {responseMessage.StatusCode} {responseMessage.ReasonPhrase}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}