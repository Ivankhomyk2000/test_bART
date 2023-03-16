using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Entity;
using Test.Data;
using Test.Services.Implementations;
using Test.Services.Contract;

namespace Test.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {

        private readonly DataContext _contextCon;
        private readonly IContactService contactService;

        public ContactController(DataContext context, IContactService contactService)
        {
            _contextCon = context;
            this.contactService = contactService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Contact>>> Get()
        {

            return Ok(await _contextCon.Contacts.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetId(int id)
        {
            Contact? contact = contactService.GetId(id);
            return contact == null ? BadRequest() : Ok(contact);
        }

        [HttpPost]
        public async Task<ActionResult> AddContact(Contact data)
        {
            var emailExist = _contextCon.Contacts.Any(h => h.Email == data.Email);
            if (data.Email == null || emailExist)
            {
                return BadRequest(); 
            }
            Contact? contact = contactService.Create(data);
            return contact == null ? BadRequest() : Ok(contact);
        }

        [HttpPut]
        public async Task<ActionResult<List<Contact>>> UpdateContact(Contact request)
        {
            Contact? contact = contactService.Update(request);
            return contact == null ? BadRequest() : Ok(contact);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Contact>>> DeleteContact(int id)
        {
            Contact? contact = contactService.Delete(id);
            return contact == null ? BadRequest() : Ok("Delete successful");
        }
    }
}
