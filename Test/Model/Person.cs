using System;
using System.Collections.Generic;

namespace Test.Model;

public partial class Person
{
    public int PersonId { get; set; }

    public int? TypeRoleId { get; set; }

    public string? LastName { get; set; }

    public string? Name { get; set; }

    public string? Patronamic { get; set; }

    public string? Login { get; set; }

    public string? Pass { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual TypeRole? TypeRole { get; set; }
}
