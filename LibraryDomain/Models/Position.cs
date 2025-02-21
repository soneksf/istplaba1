using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Models;

public partial class Position : Entity
{
    // public int Id { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Поcада працівника")]
    public string PositionName { get; set; } = null!;


    [Display(Name = "Дата початку роботи")]
    public DateOnly? StartDate { get; set; }


    [Display(Name = "Дата закінчення")]
    public DateOnly? EndDate { get; set; }



    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Працівник")]
    public int EmployeeId { get; set; }


    [Display(Name = "Працівник")]
    public virtual Employee? Employee { get; set; } = null!;
}
