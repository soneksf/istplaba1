using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Models;

public partial class Position : Entity
{
    // public int Id { get; set; }
    [Display(Name = "Позиція працівника")]
    public string PositionName { get; set; } = null!;
    [Display(Name = "Дата початку роботи")]
    public DateOnly? StartDate { get; set; }
    [Display(Name = "Дата закінчення")]
    public DateOnly? EndDate { get; set; }

    public int EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; } = null!;
}
