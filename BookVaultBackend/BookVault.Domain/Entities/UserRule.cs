using System;

namespace BookVault.Domain.Entities;

public partial class UserRule
{
	public Guid Userid { get; set; }

	public Guid Ruleid { get; set; }

	public bool Isallowed { get; set; }

	public DateTimeOffset Assignedat { get; set; }

	public Guid? Assignedby { get; set; }

	public DateTimeOffset? Expiresat { get; set; }

	public virtual User User { get; set; } = null!;

	public virtual Rule Rule { get; set; } = null!;

	public virtual User? AssignedByUser { get; set; }
}
