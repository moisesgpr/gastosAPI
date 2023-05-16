using System;
using System.Collections.Generic;

namespace gastosapi.Models;

public partial class Operation
{
    public int? IdOperations { get; set; }

    public decimal Amount { get; set; }

    public int IdUser { get; set; }

    public int IdCategory { get; set; }

    public string Description { get; set; } = null!;

    public string Created { get; set; } = null!;

    internal virtual Category? IdCategoryNavigation { get; set; }

    internal virtual User? IdUserNavigation { get; set; }
}
