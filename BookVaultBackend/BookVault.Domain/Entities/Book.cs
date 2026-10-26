using System;
using System.Collections.Generic;

namespace BookVault.Domain.Entities;

public partial class Book
{
	public Guid Id { get; set; }

	public string Isbn { get; set; } = null!;

	public string Title { get; set; } = null!;

	public string Category { get; set; } = null!;

	public string Genre { get; set; } = null!;

	public DateOnly Publicationdate { get; set; }

	public int Pagecount { get; set; }

	public string Edition { get; set; } = null!;

	public int Totalcopies { get; set; }

	public string Language { get; set; } = null!;

	public DateTime? Createdat { get; set; }

	public DateTime? Updatedat { get; set; }

	public Guid? Authorid { get; set; }

	public Guid? Publisherid { get; set; }

	public int? Shelfid { get; set; }

	public virtual Author? Author { get; set; }

	public virtual ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();

	public virtual Publisher? Publisher { get; set; }

	public virtual Shelf? Shelf { get; set; }
}