using Microsoft.AspNetCore.Mvc;
using BankAPI.Data;
namespace BankAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly BankContext _context;
    public ClientController(BankContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IEnumerable<Client> Get()
    {
        //Accedemos al metodo
        return _context.Clients.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<Client> GetById(int id)
    {
        //Accedemos al metodo
        var client = _context.Clients.Find(id);

        if (client is null)
            return NotFound();

        return client;
    }
}