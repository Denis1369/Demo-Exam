using System;
using System.Collections.Generic;

namespace Test.Model;

public partial class DeliviryPoint
{
    public int DeliviryPointId { get; set; }

    public string? Code { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    public string? Number { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public string TitleAll => City+" "+Street+" "+Number;
}
