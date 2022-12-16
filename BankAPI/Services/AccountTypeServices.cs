using BankAPI.Data;

namespace BankAPI.Services;

public class AccountTypeServices
{
    private readonly BankContext _context;

    public AccountTypeServices(BankContext context)
    {
        _context = context;
    }

    public async Task<AccountType?> GetById(int id)
    {
        return await _context.AccountTypes.FindAsync(id);
    }
}