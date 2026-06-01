using System;
using System.Collections.Generic;

namespace Testt.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? FulfilledAt { get; set; }

    public int ClientId { get; set; }

    public int StatusId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<ProductOrder> ProductOrders { get; set; } = new List<ProductOrder>();

    public virtual Status Status { get; set; } = null!;
}
