using Microsoft.AspNetCore.Authorization;

namespace BookVault.Application.Authorization;

public class RequirePermissionAttribute : AuthorizeAttribute
{
	public RequirePermissionAttribute(string permission)
	{
		Policy = $"Permission:{permission}";
	}
}
