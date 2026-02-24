using System;
using System.Collections.Generic;

namespace Test.Model;

public partial class TypeRole
{
    public int TypeRoleId { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
