using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApi.Models;
using WebApi.Persistence;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class ToyController : ControllerBase
{
    private KinderGartenContext _kinderGartenContext;

    public ToyController(KinderGartenContext kinderGartenContext)
    {
        _kinderGartenContext = kinderGartenContext;
    }
    [HttpPost]
    [Route("owner/{childId:int}")]
    public async Task<ActionResult<Toy>> AddChildAsync([FromBody] Toy toy, [FromRoute] int childId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            Child toAdd = await _kinderGartenContext.Children.Include(c => c.Toys)
                .FirstOrDefaultAsync(c => c.Id == childId);
            EntityEntry<Toy> added = await _kinderGartenContext.Toys.AddAsync(toy);
            toAdd.Toys.Add(added.Entity);
            _kinderGartenContext.Update(toAdd);
            await _kinderGartenContext.SaveChangesAsync();
            return Created($"{added.Entity.Id}", added.Entity);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete]
    [Route("{toyId:int}")]
    public async Task<ActionResult> RemoveToyAsync([FromRoute] int toyId)
    {
        try
        {
            Toy toyToRemove = await _kinderGartenContext.Toys.FirstOrDefaultAsync(t => t.Id == toyId);
            _kinderGartenContext.Remove(toyToRemove);
            await _kinderGartenContext.SaveChangesAsync();
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}