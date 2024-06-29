using System;
using System.Collections.Generic;

namespace Ecommerce.Models.db;

public partial class Product
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Text { get; set; }

    public int? Price { get; set; }

    public int? Discount { get; set; }

    public string? ImageName { get; set; }

    public int? Qty { get; set; }

    public string? Tags { get; set; }
}
