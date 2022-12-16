using System.Web;
using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data;
using TestBankAPI.Data.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace BankAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountServices accountServices;
    private readonly AccountTypeServices accountTypeServices;
    private readonly ClientServices clientServices;
    public AccountController(AccountServices accountServices, AccountTypeServices accountTypeServices, ClientServices clientServices)
    {
        this.accountServices = accountServices;
        this.accountTypeServices = accountTypeServices;
        this.clientServices = clientServices;
    }

    [HttpGet]
    public async Task<IEnumerable<AccountDtoOut>> Get()
    {
        //Accedemos al metodo
        return await accountServices.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDtoOut>> GetById(int id)
    {
        //Accedemos al metodo
        var account = await accountServices.GetDtoById(id);

        if (account is null)
            return AccountNotFound(id);

        return account;
    }

    [Authorize(Policy = "SuperAdmin")]
    [HttpPost]
    public async Task<IActionResult> Create(AccountDtoIn account)
    {
        string validationResult = await ValidateAccount(account);

        if (validationResult.Equals("valid"))
            return BadRequest(new { message = validationResult });

        var newAccount = await accountServices.Create(account);
        return CreatedAtAction(nameof(GetById), new { id = newAccount.Id }, newAccount);
    }

    [Authorize(Policy = "SuperAdmin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, AccountDtoIn account)
    {
        if (id != account.Id)
            return BadRequest(new { message = $"El accountServices de id {id} no coincide" });

        var accountToUpdate = await accountServices.GetById(id);
        if (accountToUpdate is not null)
        {
            string validationResult = await ValidateAccount(account);

            if (validationResult.Equals("valid"))
                return BadRequest(new { message = validationResult });
            await accountServices.Update(account);
            return NoContent();
        }
        else
        {
            return AccountNotFound(id);
        }
    }

    private async Task<string> ValidateAccount(AccountDtoIn account)
    {
        string result = "valid";

        var accountType = await accountTypeServices.GetById(account.AccounType);

        if (accountType is null)
            result = $"El tipo no existe";

        var clientID = account.ClientId.GetValueOrDefault();
        var client = await clientServices.GetById(clientID);
        if (client is null)
            result = $"El cliente no existe";

        return result;
    }

    [Authorize(Policy = "SuperAdmin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var accountToDelete = await accountServices.GetById(id);
        if (accountToDelete is not null)
        {
            await accountServices.Delete(id);
            return NoContent();
        }
        else
        {
            return AccountNotFound(id);
        }
    }

    public NotFoundObjectResult AccountNotFound(int id)
    {
        return NotFound(new { message = $"La cuenta de id {id} no existe" });
    }
}