namespace TestBankAPI.Data.DTOs;

public class AccountDtoIn
{
    public int Id { get; set; }

    public int? ClientId { get; set; }

    public int AccounType { get; set; }

    public decimal Balance { get; set; }
}