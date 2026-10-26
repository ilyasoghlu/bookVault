using System;
using System.Collections.Generic;

namespace BookVault.Domain.Entities;

public partial class Reader
{
    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateOnly Dateofbirth { get; set; } 

    public DateOnly Expirationdate { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

	public Guid UserId { get; set; }

	public virtual User User { get; set; } = null!;

	public string Createdby { get; set; } = null!;

    public string Updatedby { get; set; } = null!;

    public virtual ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
}
