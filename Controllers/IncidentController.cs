using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Entity;
using Test.Data;
using Test.Services.Contract;
using Test.Services.Implementations;

namespace Test.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        
        private readonly DataContext _contextInc;
        private readonly IIncidentService incidentService;
        public IncidentController(DataContext context, IIncidentService incidentService)
        {
            _contextInc = context;
            this.incidentService = incidentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Incident>>> Get()
        {

            return Ok(await _contextInc.Incidents.ToListAsync());
        }

        [HttpGet("{Name}")]
        public async Task<ActionResult<Incident>> GetInc(Guid id)
        {
            Incident? incident = incidentService.GetId(id);
            return incident == null ? BadRequest() : Ok(incident);
        }

        [HttpPost]
        public async Task<ActionResult> AddInc(Incident data)
        {
            var emailExist = _contextInc.Contacts.Any(h => h.Email == data.Account.Contact.Email);
            if (data.Account.Contact.Email == null || emailExist)
            {
                return BadRequest();
            }
            Incident? incident = incidentService.Create(data);
            return incident == null ? BadRequest() : Ok(incident);
        }

        [HttpPut]
        public async Task<ActionResult<List<Incident>>> UpdateInc(Incident request)
        {
            Incident? incident = incidentService.Update(request);
            return incident == null ? BadRequest() : Ok(incident);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Incident>>> Delete(Guid id)
        {
            Incident? incident = incidentService.Delete(id);
            return incident == null ? BadRequest() : Ok("Delete successful");
        }
    }
}
