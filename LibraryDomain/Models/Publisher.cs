using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Models;

public partial class Publisher : Entity
{
    // public int Id { get; set; }
    [Display(Name = "ПІБ")]
    public string FullName { get; set; } = null!;
    [Display(Name = "Адреса")]
    public string? Address { get; set; }

    public virtual ICollection<ResearchWork> ResearchWorks { get; set; } = new List<ResearchWork>();
}
