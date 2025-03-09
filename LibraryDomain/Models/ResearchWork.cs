using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Models;

public partial class ResearchWork : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Назва наукової роботи")]
    public string Title { get; set; } = null!;

    [Display(Name = "Працівник")]
    public int EmployeeId { get; set; }
 
    [Display(Name = "Область наукової роботи")]
    public int AreaId { get; set; }
    [Display(Name = "Область наукової роботи")]
    public virtual Area? Area { get; set; } 
    [Display(Name = "Працівник")]
    public virtual Employee? Employee { get; set; } 

}
