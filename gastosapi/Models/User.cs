using System;
using System.Collections.Generic;

namespace gastosapi.Models;

public partial class User
{
    internal int IdUser { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    internal virtual ICollection<Operation> Operations { get; set; } = new List<Operation>();
}
