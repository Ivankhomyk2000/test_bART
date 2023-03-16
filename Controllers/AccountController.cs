using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Entity;
using Test.Data;
using Test.Services.Contract;
using Azure.Core;

namespace Test.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly DataContext _contextAcc;
        private readonly IAccountService accountService;

        public AccountController(DataContext context, IAccountService accountService)
        {
            _contextAcc = context;
            this.accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Account>>> Get()
        {

            return Ok(await _contextAcc.Accounts.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetId(int id)
        {
            Account? account = accountService.GetId(id);
            return account == null ? BadRequest() : Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult> AddAccount(Account data)
        {
            var emailExist = _contextAcc.Contacts.Any(h => h.Email == data.Contact.Email);
            if (data.Contact.Email == null || emailExist)
            {
                return BadRequest();
            }
            Account? account = accountService.Create(data);
            return account == null ? BadRequest() : Ok(account);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAccount(Account request)
        {
            Account? account = accountService.Update(request);
            return account == null ? BadRequest() : Ok(account);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Account? account = accountService.Delete(id);
            return account == null ? BadRequest() : Ok("Delete successful");
        }
    }

}
