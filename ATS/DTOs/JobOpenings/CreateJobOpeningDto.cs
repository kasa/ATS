using System.ComponentModel.DataAnnotations;

namespace ATS.DTOs.JobOpenings;

public class CreateJobOpeningDto
{
	[Required]
	public string Position { get; set; }

	[Required]
	public string Description { get; set; }
}