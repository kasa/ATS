using System.ComponentModel.DataAnnotations;

namespace ATS.DTOs.Candidates;

public class UpdateCandidateDto
{
	public string Name { get; set; }

	[DataType(DataType.Upload)]
	public IFormFile File { get; set; }
}