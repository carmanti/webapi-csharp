using System.Threading.Tasks.Dataflow;
using BankAPI.Data;
using Microsoft.EntityFrameworkCore;
using TestBankAPI.Data.DTOs;

namespace BankAPI.Services;

public class AccountServices
{
    private readonly BankContext _context;
    public AccountServices(BankContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AccountDtoOut>> GetAll()
    {
        return await _context.Accounts.Select(a => new AccountDtoOut
        {
            Id = a.Id,
            AccountName = a.AccounTypeNavigation.Name,
            ClientName = a.Client.Name,
            Balance = a.Balance,
            RegDate = a.RegDate
        }).ToListAsync();
    }

    public async Task<AccountDtoOut> GetDtoById(int id)
    {
        return await _context.Accounts.Where(a => a.Id == id).Select(a => new AccountDtoOut
        {
            Id = a.Id,
            AccountName = a.AccounTypeNavigation.Name,
            ClientName = a.Client.Name,
            Balance = a.Balance,
            RegDate = a.RegDate
        }).SingleOrDefaultAsync();
    }

    public async Task<Account?> GetById(int id)
    {
        return await _context.Accounts.FindAsync(id);
    }

    public async Task<Account> Create(AccountDtoIn newAccountDTO)
    {

        var newAccount = new Account();

        newAccount.AccounType = newAccountDTO.AccounType;
        newAccount.ClientId = newAccountDTO.ClientId;
        newAccount.Balance = newAccountDTO.Balance;

        _context.Accounts.Add(newAccount);
        await _context.SaveChangesAsync();

        return newAccount;
    }

    public async Task Update(AccountDtoIn account)
    {
        var existAccount = await GetById(account.Id);

        if (existAccount is not null)
        {
            existAccount.AccounType = account.AccounType;
            existAccount.Balance = account.Balance;
            existAccount.ClientId = account.ClientId;

            await _context.SaveChangesAsync();
        }
    }

    public async Task Delete(int id)
    {
        var accountDelete = await GetById(id);

        if (accountDelete is not null)
        {
            _context.Accounts.Remove(accountDelete);
            await _context.SaveChangesAsync();
        }
    }
}