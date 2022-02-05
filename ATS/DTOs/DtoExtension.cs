using ATS.DTOs.Candidates;
using ATS.DTOs.JobOpenings;
using ATS.Models;

namespace ATS.DTOs;

public static class DtoExtension
{
	public static CandidateDto ToDto(this Candidate c, bool includeCV = false)
	{
		var candidate = new CandidateDto
		{
			CandidateId = c.CandidateId,
			Name = c.Name,
		};

		if (includeCV && c.CandidateCurriculumVitae != null)
		{
			candidate.CVDto = c.CandidateCurriculumVitae.ToDto();
		}

		return candidate;
	}

	public static CandidateCurriculumVitaeDto ToDto(this CandidateCurriculumVitae cv)
	{
		return new CandidateCurriculumVitaeDto
		{
			File = cv.File,
			Filename = cv.FileName,
		};
	}

	public static JobOpeningDto ToDto(this JobOpening jobOpening)
	{
		return new JobOpeningDto
		{
			JobOpeningId = jobOpening.JobOpeningId,
			PositionName = jobOpening.PositionName,
			Description = jobOpening.Description,
		};
	}

	public static JobApplicationDto ToDto(this JobApplication jobApplication)
	{
		return new JobApplicationDto
		{
			JobOpeningId = jobApplication.JobOpeningId,
			PositionName = jobApplication.JobOpening.PositionName,
			Description = jobApplication.JobOpening.Description,
			CandidateId = jobApplication.CandidateId,
			CandidateName = jobApplication.Candidate.Name,
		};
	}
}