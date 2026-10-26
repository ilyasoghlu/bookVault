using System;
using System.Collections.Generic;

namespace BookVault.Domain.Entities;

public partial class Shelf
{
	public int Id { get; set; }

	public int Shelvecode { get; set; }

	public int Capacity { get; set; }

	public DateTime? Createdat { get; set; }

	public DateTime? Updatedat { get; set; }

	public int Caseid { get; set; }

	public virtual BookCase Case { get; set; } = null!;

	public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}