using System.Threading.Tasks.Dataflow;
using BankAPI.Data;
using BankAPI.Data.DTOs;
using BankAPI.DataBankModels;
using Microsoft.EntityFrameworkCore;
using TestBankAPI.Data.DTOs;

namespace BankAPI.Services;

public class LoginServices
{
    private readonly BankContext _context;
    public LoginServices(BankContext context)
    {
        _context = context;
    }

    public async Task<Administrator?> GetAdmin(AdminDTO admin)
    {
        return await _context.Administrators.SingleOrDefaultAsync(x => x.Email == admin.Email && x.Pwd == admin.Pwd);
    }
}