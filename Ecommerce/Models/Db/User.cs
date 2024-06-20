using System;
using System.Collections.Generic;

namespace Ecommerce.Models.db;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string Role { get; set; } = null!;

    public int ActiveCode { get; set; }

    public bool IsActive { get; set; }

    public bool IsDelete { get; set; }

    public DateTime CreateDate { get; set; }
}
