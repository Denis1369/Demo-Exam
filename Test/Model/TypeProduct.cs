using System;
using System.Collections.Generic;

namespace Test.Model;

public partial class TypeProduct
{
    public int TypeProductId { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
