using System;
using System.Collections.Generic;

namespace BookVault.Domain.Entities;

public partial class BookCase
{
	public int Id { get; set; }

	public int Casenumber { get; set; }

	public int Capacity { get; set; }

	public DateTime? Createdat { get; set; }

	public DateTime? Updatedat { get; set; }

	public int Sectionid { get; set; }

	public virtual Section Section { get; set; } = null!;

	public virtual ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();
}
