using System.ComponentModel.DataAnnotations;

namespace ATS.DTOs.Candidates;

public class CreateCandidateDto
{
	[Required]
	public string Name { get; set; }

	[DataType(DataType.Upload)]
	public IFormFile File { get; set; }
}