using ATS.DTOs;
using ATS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobApplicationController : ControllerBase
{
	private readonly ILogger<CandidateController> _logger;
	private readonly ApplicationDbContext _context;

	public JobApplicationController(ILogger<CandidateController> logger, ApplicationDbContext context)
	{
		_logger = logger;
		_context = context;
	}

	/// <summary>
	/// Retrieve a JobApplication.
	/// </summary>
	/// <param name="jobOpeningId">Id of the JobApplication you want to retrieve the JobApplication</param>
	/// <param name="candidateId">Id of the Candidate you want to retrieve the JobApplication</param>
	/// <returns>The JobApplication</returns>
	/// <response code="200">The JobApplication</response>
	/// <response code="404">If there is no JobApplication with jobOpeningId and candidateId</response>
	[HttpGet("{jobOpeningId}/{candidateId}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<JobApplicationDto>> GetJobApplication(int jobOpeningId, int candidateId)
	{
		try
		{
			var jobApplication = await _context.JobApplication
				.Include(a => a.JobOpening)
				.Include(a => a.Candidate)
				.FirstOrDefaultAsync(a => a.JobOpeningId == jobOpeningId && a.CandidateId == candidateId);
			// .FindAsync(jobOpeningId, candidateId);
			if (jobApplication == null)
			{
				return NotFound();
			}

			return Ok(jobApplication.ToDto());
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Error fetching a list of JobOpenings");

			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}
}