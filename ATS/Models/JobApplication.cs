using System.ComponentModel.DataAnnotations;

namespace ATS.Models;

public sealed class JobApplication
{
	public int JobOpeningId { get; set; }
	public int CandidateId { get; set; }

	[Required]
	public DateTime ApplicationDate { get; set; }

	public JobOpening JobOpening { get; set; }
	public Candidate Candidate { get; set; }
}