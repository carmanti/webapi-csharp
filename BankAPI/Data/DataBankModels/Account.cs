﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BankAPI.Data;

public partial class Account
{
    public int Id { get; set; }

    public int? ClientId { get; set; }

    public int AccounType { get; set; }

    public decimal Balance { get; set; }

    public DateTime RegDate { get; set; }

    [JsonIgnore]
    public virtual AccountType AccounTypeNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<BankTransaction> BankTransactions { get; } = new List<BankTransaction>();

    [JsonIgnore]
    public virtual Client Client { get; set; } = null!;
}
