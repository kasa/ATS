namespace ATS.DTOs;

public class JobApplicationDto
{
	public int JobOpeningId { get; internal set; }
	public string PositionName { get; internal set; }
	public string Description { get; internal set; }
	public int CandidateId { get; internal set; }
	public string CandidateName { get; internal set; }
}