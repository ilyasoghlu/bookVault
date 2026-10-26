
using System.Collections.Generic;

namespace BookVault.Domain.Entities;

public partial class Borrowing
{
    public int Id { get; set; }

    public DateOnly Duedate { get; set; }

    public DateOnly? Returndate { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public int Readerid { get; set; }

    public Guid Bookid { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Reader Reader { get; set; } = null!;
}