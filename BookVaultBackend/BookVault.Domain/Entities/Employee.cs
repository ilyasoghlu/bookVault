using System;
using System.Collections.Generic;

namespace BookVault.Domain.Entities;

public partial class Employee
{
    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    // for user access 
	public Guid UserId { get; set; }
	public virtual User User { get; set; } = null!;

	public string Employeeposition { get; set; } = null!;

    public DateOnly Hiredate { get; set; }

    public DateOnly? Enddate { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

}
