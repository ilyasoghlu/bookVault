using System;
using System.Collections.Generic;

namespace BookVault.Domain.Entities;

public partial class Room
{
	public int Id { get; set; }

	public string Title { get; set; } = null!;

	public int Roomnumber { get; set; }

	public int Capacity { get; set; }

	public int Roomfloor { get; set; }

	public string Description { get; set; } = null!;

	public DateTime? Createdat { get; set; }

	public DateTime? Updatedat { get; set; }

	public int? Employeeid { get; set; }

	public virtual Employee? Employee { get; set; }

	public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
}
