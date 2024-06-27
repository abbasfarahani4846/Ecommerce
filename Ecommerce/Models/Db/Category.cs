using System;
using System.Collections.Generic;

namespace Ecommerce.Models.db;

public partial class Category
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public bool IsDelete { get; set; }
}
