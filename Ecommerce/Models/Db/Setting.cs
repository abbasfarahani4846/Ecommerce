using System;
using System.Collections.Generic;

namespace Ecommerce.Models.db;

public partial class Setting
{
    public int Id { get; set; }

    public decimal? Shipping { get; set; }
}
