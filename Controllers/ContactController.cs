using CMS_.Introduction.Models;
using CMS_.Introduction.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_.Introduction.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactService _contactService;
    public ContactsController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet(Name = "Index")]
    public IActionResult Get()
    {
        var contacts = _contactService.Get();
        return Ok(contacts);
    }

    [HttpPost(Name = "Index")]
    public IActionResult Post([FromBody] ContactModel contact)
    {
        _contactService.Add(contact);
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var contact = _contactService.Get(id);
        if(contact is null)
        {
            return NotFound();
        }
        return Ok(contact);
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, [FromBody] ContactModel contact)
    {
        contact.Id = id;
        _contactService.Update(contact);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _contactService.Delete(id);
        return Ok();
    }
}
