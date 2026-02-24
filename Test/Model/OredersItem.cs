using System;
using System.Collections.Generic;

namespace Test.Model;

public partial class OredersItem
{
    public int OredersItemId { get; set; }

    public int? OrdersId { get; set; }

    public string? ProductId { get; set; }

    public int? Count { get; set; }

    public virtual Order? Orders { get; set; }

    public virtual Product? Product { get; set; }
}
