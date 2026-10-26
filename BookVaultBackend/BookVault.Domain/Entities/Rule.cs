using System;
using System.Collections.Generic;

namespace BookVault.Domain.Entities;

public partial class Rule
{
	public Guid Id { get; set; }

	public string Code { get; set; } = null!;

	public string Rulename { get; set; } = null!;

	public string? Description { get; set; }

	public bool Isactive { get; set; }

	public DateTime Createdat { get; set; }

	public DateTime? Updatedat { get; set; }

	public virtual ICollection<UserRule> UserRules { get; set; } = new List<UserRule>();
}
