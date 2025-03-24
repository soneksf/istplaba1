using LibraryDomain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Models;

public partial class Position : Entity
{
    
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Поcада працівника")]

    public string PositionName { get; set; } = null!;


    [Display(Name = "Дата початку роботи")]
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [PastOrYesterdayDate(ErrorMessage = "Дата початку не може бути пізніше сьогоднішнього дня.")]
    public DateOnly? StartDate { get; set; }


    [Display(Name = "Дата закінчення")]
    [DateGreaterThan(nameof(StartDate), ErrorMessage = "Дата закінчення має бути пізніше дати початку.")]
    public DateOnly? EndDate { get; set; }



    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Працівник")]
    public int EmployeeId { get; set; }


    [Display(Name = "Працівник")]
    public virtual Employee? Employee { get; set; } = null!;
}
