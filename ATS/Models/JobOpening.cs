using System.ComponentModel.DataAnnotations;

namespace ATS.Models;

public sealed class JobOpening
{
	public int JobOpeningId { get; set; }

	[Required]
	public string PositionName { get; set; }

	[Required]
	public string Description { get; set; }

	public List<JobApplication> JobCandidates { get; } = new();
}
