using BookVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BookVault.Application.Authorization;

public class PermissionAuthorizationHandler
	: AuthorizationHandler<PermissionRequirement>
{
	private readonly IRuleService _ruleService;

	public PermissionAuthorizationHandler(IRuleService ruleService)
	{
		_ruleService = ruleService;
	}

	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		PermissionRequirement requirement)
	{
		var userIdClaim = context.User.FindFirst(
			ClaimTypes.NameIdentifier);

		if (userIdClaim is null)
			return;

		if (!Guid.TryParse(userIdClaim.Value, out var userId))
			return;

		var hasPermission = await _ruleService.HasPermissionAsync(
			userId,
			requirement.Permission);

		if (hasPermission)
		{
			context.Succeed(requirement);
		}
	}
}
