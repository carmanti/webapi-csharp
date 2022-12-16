using BankAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public class ClientServices
{
    private readonly BankContext _context;
    public ClientServices(BankContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Client>> GetAll()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<Client?> GetById(int id)
    {
        return await _context.Clients.FindAsync(id);
    }

    public async Task<Client> Create(Client newClient)
    {
        _context.Clients.Add(newClient);
        await _context.SaveChangesAsync();

        return newClient;
    }

    public async Task Update(int id, Client client)
    {
        var existClient = await GetById(id);

        if (existClient is not null)
        {
            existClient.Name = client.Name;
            existClient.PhoneNumber = client.PhoneNumber;
            existClient.Email = client.Email;

            await _context.SaveChangesAsync();
        }
    }

    public async Task Delete(int id)
    {
        var clientDelete = await GetById(id);

        if (clientDelete is not null)
        {
            _context.Clients.Remove(clientDelete);
            await _context.SaveChangesAsync();
        }
    }


}