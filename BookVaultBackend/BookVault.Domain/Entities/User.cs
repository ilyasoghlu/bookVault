using System;
using System.Collections.Generic;

namespace BookVault.Domain.Entities;

public partial class User
{
	public Guid Id { get; set; }

	public string Firstname { get; set; } = null!;

	public string Lastname { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string Username { get; set; } = null!;

	public string Passwordhash { get; set; } = null!;

	public bool Isactive { get; set; }

	public bool Isdeleted { get; set; }

	public DateTime Createdat { get; set; }

	public DateTime? Updatedat { get; set; }

	public virtual ICollection<UserRule> UserRules { get; set; } = new List<UserRule>();

	public virtual ICollection<UserRule> AssignedUserRules { get; set; } = new List<UserRule>();
}
