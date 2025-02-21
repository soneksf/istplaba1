using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Models;

public partial class Department : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display (Name = "Назва катедри")]
//    public int Id { get; set; }

    public string? DepartmentName { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
