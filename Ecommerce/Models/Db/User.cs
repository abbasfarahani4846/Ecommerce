using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Models.db;

[Table("User")]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string? Username { get; set; }

    [StringLength(100)]
    public string? Password { get; set; }

    [StringLength(50)]
    public string Role { get; set; } = null!;

    public int ActiveCode { get; set; }

    public bool IsActive { get; set; }

    public bool IsDelete { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }
}
