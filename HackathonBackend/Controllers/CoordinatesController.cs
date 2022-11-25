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
public class CoordinatesController : ControllerBase
{
    private readonly DestinyContext Database;

    public CoordinatesController(DestinyContext Database)
    {
        this.Database = Database;
    }

    // GET: api/Coordinates
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Coordinate>>> GetCoordinates()
    {
        if (Database.Coordinates == null)
        {
            return NotFound();
        }

        return await Database.Coordinates.ToListAsync();
    }

    // GET: api/Coordinates/5
    [HttpGet("{Id}")]
    public async Task<ActionResult<Coordinate>> GetCoordinate(int Id)
    {
        if (Database.Coordinates == null)
        {
            return NotFound();
        }

        var Coordinate = await Database.Coordinates.FindAsync(Id);

        if (Coordinate == null)
        {
            return NotFound();
        }

        return Coordinate;
    }

    // PUT: api/Coordinates/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{Id}")]
    public async Task<IActionResult> PutCoordinate(int Id, Coordinate Coordinate)
    {
        if (Id != Coordinate.Id)
        {
            return BadRequest();
        }

        Database.Entry(Coordinate).State = EntityState.Modified;

        try
        {
            await Database.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CoordinateExists(Id))
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

    // POST: api/Coordinates
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Coordinate>> PostCoordinate(Coordinate Coordinate)
    {
        if (Database.Coordinates == null)
        {
            return Problem("Entity set 'DestinyContext.Coordinates'  is null.");
        }

        Database.Coordinates.Add(Coordinate);
        await Database.SaveChangesAsync();

        return CreatedAtAction("GetCoordinate", new { Id = Coordinate.Id }, Coordinate);
    }

    // DELETE: api/Coordinates/5
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteCoordinate(int Id)
    {
        if (Database.Coordinates == null)
        {
            return NotFound();
        }

        var Coordinate = await Database.Coordinates.FindAsync(Id);

        if (Coordinate == null)
        {
            return NotFound();
        }

        Database.Coordinates.Remove(Coordinate);
        await Database.SaveChangesAsync();

        return NoContent();
    }

    private bool CoordinateExists(int Id)
    {
        return (Database.Coordinates?.Any(C => C.Id == Id)).GetValueOrDefault();
    }
}
