using System;
using System.Collections.Generic;

namespace gastosapi.Models;

public partial class Category
{
    public int? IdCategory { get; set; }

    public string CategoryName { get; set; } = null!;

    internal virtual ICollection<Operation> Operation { get; set; } = new List<Operation>();
}
