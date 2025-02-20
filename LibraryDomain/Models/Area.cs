using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Models;

public partial class Area : Entity
{
    //  public int Id { get; set; }
    [Display(Name = "Область Дослідницької роботи")]
    public string AreaName { get; set; } = null!;

    public virtual ICollection<ResearchWork> ResearchWorks { get; set; } = new List<ResearchWork>();
}
