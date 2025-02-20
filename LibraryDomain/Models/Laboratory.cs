using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Models;

public partial class Laboratory : Entity
{
    // public int Id { get; set; }
    [Display(Name = "Номер лабораторії")]
    public string LabNumber { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
