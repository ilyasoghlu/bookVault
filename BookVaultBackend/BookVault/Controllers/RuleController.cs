using BookVault.Application.Authorization;
using BookVault.Application.DTOs.Rules;
using BookVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace BookVault.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RuleController : ControllerBase
{
	private readonly IRuleService _ruleService;

	public RuleController(IRuleService ruleService)
	{
		_ruleService = ruleService;
	}


	[HttpPost]
	[RequirePermission("RULE_CREATE")]
	public async Task<IActionResult> Create(
		[FromBody] CreateRuleRequest request)
	{
		await _ruleService.CreateAsync(
			request.Code,
			request.RuleName,
			request.Description);

		return StatusCode(
			StatusCodes.Status201Created,
			new
			{
				message = "Rule created successfully."
			});
	}

	[HttpPost("assign")]
	[RequirePermission("RULE_ASSIGN")]
	public async Task<IActionResult> AssignPermission(
		[FromBody] AssignPermissionRequest request)
	{
		var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

		if (userIdClaim is null)
			return Unauthorized();

		if (!Guid.TryParse(userIdClaim.Value, out var assignedBy))
			return Unauthorized();

		await _ruleService.AssignPermissionAsync(
			request.UserId,
			request.RuleCode,
			assignedBy: assignedBy,
			expiresAt: request.ExpiresAt);

		return Ok(new
		{
			message = "Permission assigned successfully."
		});
	}

}
