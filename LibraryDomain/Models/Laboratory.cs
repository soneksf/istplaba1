using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Models;

public partial class Laboratory : Entity
{

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Range(1, int.MaxValue, ErrorMessage = "Номер лабораторії має бути невід’ємним цілим числом")]
    [Display(Name = "Номер лабораторії")]
    public int LabNumber { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
