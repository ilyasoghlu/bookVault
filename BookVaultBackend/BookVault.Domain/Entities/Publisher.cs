using System;
using System.Collections.Generic;

namespace BookVault.Domain.Entities;

public partial class Publisher
{
    public Guid Id { get; set; }

    public string Publishername { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Website { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
