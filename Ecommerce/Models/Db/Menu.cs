using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Models.db;

[Table("Menu")]
public partial class Menu
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Title { get; set; } = null!;

    [StringLength(200)]
    public string? Link { get; set; }

    [StringLength(50)]
    public string Type { get; set; } = null!;
}
