using System;
using System.Collections.Generic;

namespace APIAbyster.Models;

public partial class Operation
{
    public decimal IdOperation { get; set; }

    public decimal IdUser { get; set; }

    public decimal IdCategorie { get; set; }

    public DateTime? DateOperation { get; set; }

    public string? LibelleOperation { get; set; }

    public string? MontantOperation { get; set; }

    public virtual Categorie IdCategorieNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
