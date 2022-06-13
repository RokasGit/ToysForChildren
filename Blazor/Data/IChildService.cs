using Blazor.Models;

namespace Blazor.Data;

public interface IChildService
{
    Task AddChildAsync(Child child);
    Task<IList<Child>> GetChildrenAsync(string? gender, string? favorite);
}