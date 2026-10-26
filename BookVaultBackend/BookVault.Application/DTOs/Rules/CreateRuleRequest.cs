namespace BookVault.Application.DTOs.Rules;

public class CreateRuleRequest
{
	public string Code { get; set; } = null!;

	public string RuleName { get; set; } = null!;

	public string? Description { get; set; }
}
