using System;
using System.Collections.Generic;

namespace APIAbyster.Models;

public partial class Categorie
{
    public decimal IdCategorie { get; set; }

    public string? LibelleCategorie { get; set; }

    public DateTime? CreateCategorie { get; set; }

    public int? ArchivedCategorie { get; set; }

    public virtual ICollection<Operation> Operations { get; } = new List<Operation>();
}
