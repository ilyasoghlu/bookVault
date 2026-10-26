using System;
using System.Collections.Generic;

namespace BookVault.Domain.Entities;

public partial class Author
{
    public Guid Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public DateOnly? Deathdate { get; set; }

    public string Nationality { get; set; } = null!;

    public string Biography { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public Guid? Publisherid { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual Publisher? Publisher { get; set; }
}
