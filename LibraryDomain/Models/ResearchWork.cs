using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Models;

public partial class ResearchWork : Entity
{
    // public int Id { get; set; }
    [Display(Name = "Назва дослідницької роботи")]
    public string Title { get; set; } = null!;
    [Display(Name = "Працівник")]
    public int EmployeeId { get; set; }
    //[Display(Name = "Автор")]
  //  public int PublisherId { get; set; }
    [Display(Name = "Область дослідницької роботи")]
    public int AreaId { get; set; }

    public virtual Area? Area { get; set; } = null!;

    public virtual Employee? Employee { get; set; } = null!;

   // public virtual Publisher? Publisher { get; set; } = null!;
}
