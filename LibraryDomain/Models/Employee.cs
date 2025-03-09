using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace LibraryDomain.Models;
using LibraryDomain.Validation;
public partial class Employee : Entity
{
   
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "ПІБ")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Факультет")]
    public string? Faculty { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [PastOrYesterdayDate(ErrorMessage = "Дата початку роботи не може бути сьогодні або в майбутньому")]
    [Display(Name = "Дата початку роботи")]
    public DateOnly StartDate { get; set; }

    [Display(Name = "Дата закінчення")]
    [DateGreaterThan("StartDate", ErrorMessage = "Дата закінчення не може бути меншою за дату початку")]
    public DateOnly? EndDate { get; set; }

    [Display(Name = "Катедра")]
    public int DepartmentId { get; set; }
 
    [Display(Name = "Лабораторія")]
    public int? LabId { get; set; }

    [Display(Name = "Катедра")]
    public virtual Department? Department { get; set; } = null!;
    [Display(Name = "Лабораторія")]
    public virtual Laboratory? Lab { get; set; }

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();

    public virtual ICollection<ResearchWork> ResearchWorks { get; set; } = new List<ResearchWork>();
}
