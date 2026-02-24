using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace Test.Model;

public partial class Order
{
    public int OrdersId { get; set; }

    public DateOnly? DateOrder { get; set; }

    public DateOnly? DateDeliviry { get; set; }

    public int? DeliviryPointId { get; set; }

    public int? PersonId { get; set; }

    public string? Code { get; set; }

    public string? Status { get; set; }

    public virtual DeliviryPoint? DeliviryPoint { get; set; }

    public virtual ICollection<OredersItem> OredersItems { get; set; } = new List<OredersItem>();

    public virtual Person? Person { get; set; }

    public string DateOrderS => (DateOrder.Value.Day + "-" + DateOrder.Value.Month + "-" + DateOrder.Value.Year).ToString();

    public string DateDeliviryS => (DateDeliviry.Value.Day+"-" + DateDeliviry.Value.Month + "-" 
        + DateDeliviry.Value.Year).ToString();
}
