using System;
using System.Collections.Generic;

namespace gastosapi.Models;

public partial class Category
{
    internal int? IdCategory { get; set; }

    public string CategoryName { get; set; } = null!;

    internal virtual ICollection<Operation> Operations { get; set; } = new List<Operation>();
}
