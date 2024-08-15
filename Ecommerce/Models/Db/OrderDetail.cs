using System;
using System.Collections.Generic;

namespace Ecommerce.Models.db;

public partial class OrderDetail
{
    public int Id { get; set; }

    public string ProductTitle { get; set; } = null!;

    public decimal ProductPrice { get; set; }

    public decimal Count { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }
}
