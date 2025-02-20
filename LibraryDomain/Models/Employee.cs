using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace LibraryDomain.Models;

public partial class Employee : Entity
{
    // public int Id { get; set; }

    [Display(Name = "ПІБ")]
    public string FullName { get; set; } = null!;
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

    public virtual Department? Department { get; set; } = null!;

    public virtual Laboratory? Lab { get; set; }

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();

    public virtual ICollection<ResearchWork> ResearchWorks { get; set; } = new List<ResearchWork>();
}
