using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HackathonBackend.Models;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HackathonBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly DestinyContext Database;

    public UsersController(DestinyContext Database)
    {
        this.Database = Database;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        if (Database.Users == null)
        {
            return NotFound();
        }

        return await Database.Users.ToListAsync();
    }

    // GET: api/Users/Find/{UserNameOrEmail}
    [HttpGet("Find/{UserNameOrEmail}")]
    public async Task<ActionResult<User>> GetUser(string UserNameOrEmail)
    {
        if (Database.Users == null)
        {
            return NotFound();
        }

        var Query = await Database.Users
            .FirstOrDefaultAsync(User => User.UserName == UserNameOrEmail || User.Email == UserNameOrEmail);

        if (Query == null)
        {
            return NotFound();
        }

        return Query;
    }

    // GET: api/Users/5
    [HttpGet("{Id}")]
    public async Task<ActionResult<User>> GetUser(int Id)
    {
        if (Database.Users == null)
        {
            return NotFound();
        }

        var User = await Database.Users.FindAsync(Id);

        if (User == null)
        {
            return NotFound();
        }

        return User;
    }

    // PUT: api/Users/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{Id}")]
    public async Task<IActionResult> PutUser(int Id, User User)
    {
        if (Id != User.Id)
        {
            return BadRequest();
        }

        Database.Entry(User).State = EntityState.Modified;

        try
        {
            await Database.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(Id))
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

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User User)
    {
        if (Database.Users == null)
        {
            return Problem("Entity set 'DestinyContext.Users'  is null.");
        }

        User.Password = new PasswordHasher<object>().HashPassword(null, User.Password);

        Database.Users.Add(User);
        await Database.SaveChangesAsync();

        return CreatedAtAction("GetUser", new { Id = User.Id }, User);
    }

    // DELETE: api/Users/5
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteUser(int Id)
    {
        if (Database.Users == null)
        {
            return NotFound();
        }

        var User = await Database.Users.FindAsync(Id);

        if (User == null)
        {
            return NotFound();
        }

        Database.Users.Remove(User);
        await Database.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(int Id)
    {
        return (Database.Users?.Any(User => User.Id == Id)).GetValueOrDefault();
    }
}
