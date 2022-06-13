using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApi.Models;
using WebApi.Persistence;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class ChildController : ControllerBase
{
    private KinderGartenContext _kinderGartenContext;

    public ChildController(KinderGartenContext kinderGartenContext)
    {
        _kinderGartenContext = kinderGartenContext;
    }

    [HttpGet]
    public async Task<ActionResult<IList<Child>>> GetChildrenAsync([FromQuery] string? gender, [FromQuery] string? favorite)
    {
        try
        {
            IList<Child> children = await _kinderGartenContext.Children.Include(c => c.Toys).ToListAsync();
            if (gender != null)
            {
                children = children.Where(c => c.Gender.ToLower().Contains(gender.ToLower())).ToList();
            }

            Console.WriteLine(favorite);
            if (favorite != null)
            {
                if (favorite.Equals("Favorites"))
                {
                    foreach (var child in children)
                    {
                        if (child.Toys != null && child.Toys.Any())
                        {
                           child.Toys = child.Toys.Where(t => t.IsFavourite == true).ToList();
                        }
                    }
                }else if (favorite.Equals("Non-Favorites"))
                {
                    foreach (var child in children)
                    {
                        if (child.Toys != null && child.Toys.Any())
                        {
                            child.Toys = child.Toys.Where(t => t.IsFavourite == false).ToList();
                        }
                    }
                }
            }

            return Ok(children);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    [HttpPost]
    public async Task<ActionResult<Child>> AddChildAsync([FromBody] Child child)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            EntityEntry<Child> added = await _kinderGartenContext.Children.AddAsync(child);
            await _kinderGartenContext.SaveChangesAsync();
            return Created($"/{added.Entity.Id}", added.Entity);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}