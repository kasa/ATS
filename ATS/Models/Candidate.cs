using System.ComponentModel.DataAnnotations;

namespace ATS.Models;

public sealed class Candidate
{
	public int CandidateId { get; set; }

	[Required]
	public string Name { get; set; }

	public CandidateCurriculumVitae CandidateCurriculumVitae { get; set; }
	public List<JobApplication> JobApplications { get; } = new();
}
