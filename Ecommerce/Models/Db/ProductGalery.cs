﻿using System;
using System.Collections.Generic;

namespace Ecommerce.Models.db;

public partial class ProductGalery
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string? ImageName { get; set; }
}
