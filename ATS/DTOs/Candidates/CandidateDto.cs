namespace ATS.DTOs.Candidates;

public class CandidateDto
{
	public int CandidateId { get; set; }
	public string Name { get; set; }

	public CandidateCurriculumVitaeDto CVDto { get; set; }
}