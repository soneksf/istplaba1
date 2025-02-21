using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Models;

public partial class Area : Entity
{
    //  public int Id { get; set; }
    [Required (ErrorMessage ="Поле не повинно бути порожнім")]
    [Display(Name = "Область дослідницької роботи")]
    public string AreaName { get; set; } = null!;

    public virtual ICollection<ResearchWork> ResearchWorks { get; set; } = new List<ResearchWork>();
}
