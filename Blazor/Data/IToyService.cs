using Blazor.Models;

namespace Blazor.Data;

public interface IToyService
{
    Task AddToyAsync(Toy toy, int childId);
    Task RemoveToy(int toyId);
}