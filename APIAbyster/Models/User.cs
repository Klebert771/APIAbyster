using System;
using System.Collections.Generic;

namespace APIAbyster.Models;

public partial class User
{
    public decimal IdUser { get; set; }

    public string? NomUser { get; set; }

    public string? PrenomUser { get; set; }

    public string? EmailUser { get; set; }

    public bool? StatusUser { get; set; }

    public string? MdpUser { get; set; }

    public string? RoleUser { get; set; }

    public DateTime? CreatedUser { get; set; }

    public int? ArchivedUser { get; set; }

    public virtual ICollection<Operation> Operations { get; } = new List<Operation>();
}
