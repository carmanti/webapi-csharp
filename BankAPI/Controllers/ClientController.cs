using System.Web;
using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace BankAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly ClientServices _service;
    public ClientController(ClientServices client)
    {
        _service = client;
    }

    [HttpGet]
    public async Task<IEnumerable<Client>> Get()
    {
        //Accedemos al metodo
        return await _service.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetById(int id)
    {
        //Accedemos al metodo
        var client = await _service.GetById(id);

        if (client is null)
            return ClientNotFound(id);

        return client;
    }

    [Authorize(Policy = "SuperAdmin")]
    [HttpPost]
    public async Task<IActionResult> Create(Client client)
    {
        var newClient = await _service.Create(client);
        return CreatedAtAction(nameof(GetById), new { id = newClient.Id }, newClient);
    }

    [Authorize(Policy = "SuperAdmin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Client client)
    {
        if (id != client.Id)
            return BadRequest(new { message = $"El cliente de id {id} no coincide" });

        var clientToUpdate = await _service.GetById(id);
        if (clientToUpdate is not null)
        {
            await _service.Update(id, client);
            return NoContent();
        }
        else
        {
            return ClientNotFound(id);
        }
    }

    [Authorize(Policy = "SuperAdmin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var clientToDelete = await _service.GetById(id);
        if (clientToDelete is not null)
        {
            await _service.Delete(id);
            return NoContent();
        }
        else
        {
            return ClientNotFound(id);
        }
    }

    public NotFoundObjectResult ClientNotFound(int id)
    {
        return NotFound(new { message = $"El cliente de id {id} no existe" });
    }
}