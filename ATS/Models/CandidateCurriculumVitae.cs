using System.ComponentModel.DataAnnotations;

namespace ATS.Models;

public sealed class CandidateCurriculumVitae
{
	public int CandidateId { get; set; }

	[Required]
	public string FileName { get; set; } = null!;

	[Required]
	public byte[] File { get; set; } = null!;

	public Candidate Candidate { get; set; } = null!;
}
