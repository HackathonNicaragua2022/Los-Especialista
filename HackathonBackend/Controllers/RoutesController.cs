using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HackathonBackend;
using HackathonBackend.Models;

namespace HackathonBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoutesController : ControllerBase
{
    private readonly DestinyContext Database;

    public RoutesController(DestinyContext Database)
    {
        this.Database = Database;
    }

    // GET: api/Routes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Routes>>> GetRoutes()
    {
        if (Database.Routes == null)
        {
            return NotFound();
        }

        return await Database.Routes.ToListAsync();
    }

    // GET: api/Routes/5
    [HttpGet("{Id}")]
    public async Task<ActionResult<Routes>> GetRoutes(string Id)
    {
        if (Database.Routes == null)
        {
            return NotFound();
        }

        var Routes = await Database.Routes.FindAsync(Id);

        if (Routes == null)
        {
            return NotFound();
        }

        return Routes;
    }

    // PUT: api/Routes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{Id}")]
    public async Task<IActionResult> PutRoutes(string Id, Routes Routes)
    {
        if (Id != Routes.Id)
        {
            return BadRequest();
        }

        Database.Entry(Routes).State = EntityState.Modified;

        try
        {
            await Database.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RoutesExists(Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Routes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Routes>> PostRoutes(Routes Routes)
    {
        if (Database.Routes == null)
        {
            return Problem("Entity set 'DestinyContext.Routes'  is null.");
        }

        Database.Routes.Add(Routes);

        try
        {
            await Database.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (RoutesExists(Routes.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetRoutes", new { Id = Routes.Id }, Routes);
    }

    // DELETE: api/Routes/5
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteRoutes(string Id)
    {
        if (Database.Routes == null)
        {
            return NotFound();
        }

        var Routes = await Database.Routes.FindAsync(Id);

        if (Routes == null)
        {
            return NotFound();
        }

        Database.Routes.Remove(Routes);

        await Database.SaveChangesAsync();

        return NoContent();
    }

    private bool RoutesExists(string Id)
    {
        return (Database.Routes?.Any(R => R.Id == Id)).GetValueOrDefault();
    }
}
