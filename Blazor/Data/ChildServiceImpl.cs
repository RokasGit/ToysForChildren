using System.Text;
using System.Text.Json;
using Blazor.Models;

namespace Blazor.Data;

public class ChildServiceImpl : IChildService
{
    private string uri = "https://localhost:7096";
    private HttpClient _client;

    public ChildServiceImpl()
    {
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
        {
            return true;
        };
        _client = new HttpClient(clientHandler);
    }
    
    public async Task AddChildAsync(Child child)
    {
        string childAsJson = JsonSerializer.Serialize(child);
        StringContent content = new StringContent(childAsJson, Encoding.UTF8, "application/json");
        HttpResponseMessage responseMessage = await _client.PostAsync($"{uri}/Child", content);
        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {responseMessage.StatusCode} {responseMessage.ReasonPhrase}");
        }
    }

    public async Task<IList<Child>> GetChildrenAsync(string? gender, string? favorite)
    {
        HttpResponseMessage responseMessage = await _client.GetAsync($"{uri}/Child?gender={gender}&favorite={favorite}");
        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {responseMessage.StatusCode} {responseMessage.ReasonPhrase}");
        }

        string result = await responseMessage.Content.ReadAsStringAsync();
        IList<Child> children = JsonSerializer.Deserialize<IList<Child>>(result,
            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        return children;
    }
}