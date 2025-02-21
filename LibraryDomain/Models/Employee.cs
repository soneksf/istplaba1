using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace LibraryDomain.Models;

public partial class Employee : Entity
{
    // public int Id { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "ПІБ")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Факультет")]
    public string? Faculty { get; set; }

    [Display(Name = "Дата початку роботи")]
    public DateOnly? StartDate { get; set; }

    [Display(Name = "Дата закінчення")]
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
