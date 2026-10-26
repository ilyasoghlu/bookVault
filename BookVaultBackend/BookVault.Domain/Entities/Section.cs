using System;
using System.Collections.Generic;

namespace BookVault.Domain.Entities;

public partial class Section
{
	public int Id { get; set; }

	public string Title { get; set; } = null!;

	public int Sectionnumber { get; set; }

	public int Capacity { get; set; }

	public int Sectionfloor { get; set; }

	public string Description { get; set; } = null!;

	public DateTime? Createdat { get; set; }

	public DateTime? Updatedat { get; set; }

	public int? Employeeid { get; set; }

	public int Roomid { get; set; }

	public virtual Employee? Employee { get; set; }

	public virtual Room Room { get; set; } = null!;

	public virtual ICollection<BookCase> BookCases { get; set; } = new List<BookCase>();
}
