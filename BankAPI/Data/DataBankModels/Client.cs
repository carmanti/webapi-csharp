using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankAPI.Data;

public partial class Client
{
    public int Id { get; set; }

    [MaxLength(200, ErrorMessage = "Debe ser menor a 200 caracteres")]
    public string Name { get; set; } = null!;

    [MaxLength(40, ErrorMessage = "Debe ser menor a 40 caracteres")]
    public string PhoneNumber { get; set; } = null!;

    [MaxLength(50, ErrorMessage = "Debe ser menor a 50 caracteres")]
    [EmailAddress(ErrorMessage = "Formato incorrecto")]
    public string? Email { get; set; }

    public DateTime RegDate { get; set; }

    [JsonIgnore]
    public virtual ICollection<Account> Accounts { get; } = new List<Account>();
}
