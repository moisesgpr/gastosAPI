using System;
using System.Collections.Generic;

namespace gastosapi.Models;

public partial class Operation
{
    public int? IdOperation { get; set; }

    public decimal Amount { get; set; }

    public int IdUser { get; set; }

    public int IdCategory { get; set; }

    public string Description { get; set; } = null!;

    public DateTime Created { get; set; }

    internal virtual Category? IdCategoryNavigation { get; set; }

    internal virtual User? IdUserNavigation { get; set; }
}
