using System;
using System.Collections.Generic;

namespace BankAPI.Data;

public partial class Account
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int AccounType { get; set; }

    public decimal Balance { get; set; }

    public DateTime RegDate { get; set; }

    public virtual AccountType AccounTypeNavigation { get; set; } = null!;

    public virtual ICollection<BankTransaction> BankTransactions { get; } = new List<BankTransaction>();

    public virtual Client Client { get; set; } = null!;
}
