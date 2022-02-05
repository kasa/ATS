using ATS.DTOs;
using ATS.DTOs.Candidates;
using ATS.DTOs.JobOpenings;
using ATS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ATS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobOpeningController : ControllerBase
{
	private readonly ILogger<CandidateController> _logger;
	private readonly ApplicationDbContext _context;

	public JobOpeningController(ILogger<CandidateController> logger, ApplicationDbContext context)
	{
		_logger = logger;
		_context = context;
	}

	/// <summary>
	/// List all JobOpenings.
	/// </summary>
	/// <returns>A list of JobOpenings</returns>
	/// <response code="200">Returns a list of JobOpenings</response>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<JobOpeningDto>>> ListJobOpenings()
	{
		try
		{
			var jobOpenings = await _context.JobOpening.ToListAsync();

			return Ok(jobOpenings.Select(j => j.ToDto()));
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Error fetching a list of JobOpenings");

			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	/// Retrieve a JobOpening.
	/// </summary>
	/// <param name="jobOpeningId">Id of the jobOpeningId you want to retrieve</param>
	/// <returns>The JobOpening</returns>
	/// <response code="200">The JobOpening</response>
	/// <response code="404">If there is no JobOpening with jobOpeningId</response>
	[HttpGet("{jobOpeningId}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<JobOpeningDto>> GetJobOpening(int jobOpeningId)
	{
		try
		{
			var jobOpening = await _context.JobOpening.FindAsync(jobOpeningId);

			if (jobOpening == null)
			{
				return NotFound();
			}

			return Ok(jobOpening.ToDto());
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Error fetching JobOpening {JobOpening}", jobOpeningId);

			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	/// Create a new JobOpening.
	/// </summary>
	/// <param name="request">JobOpening details</param>
	/// <returns>The new JobOpening</returns>
	/// <response code="200">The new JobOpening</response>
	[HttpPost]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public async Task<ActionResult<JobOpeningDto>> CreateJobOpening(CreateJobOpeningDto request)
	{
		try
		{
			var jobOpening = new JobOpening
			{
				PositionName = request.Position,
				Description = request.Description
			};

			await _context.JobOpening.AddAsync(jobOpening);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetJobOpening), new { jobOpeningId = jobOpening.JobOpeningId }, jobOpening.ToDto());
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Could not create a new JobOpening");

			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	/// Update a JobOpening.
	/// </summary>
	/// <param name="jobOpeningId">Id of the JobOpening to be updated</param>
	/// <param name="request">JobOpening details</param>
	/// <response code="200">If successfully updated</response>
	/// <response code="404">If there is no JobOpening with jobOpeningId</response>
	[HttpPut("{jobOpeningId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult> UpdateJobOpening(int jobOpeningId, UpdateJobOpeningDto request)
	{
		try
		{
			var jobOpening = await _context.JobOpening.FindAsync(jobOpeningId);
			if (jobOpening == null)
			{
				return NotFound();
			}

			if (request.PositionName != null)
			{
				jobOpening.PositionName = request.PositionName;
			}
			if (request.Description != null)
			{
				jobOpening.Description = request.Description;
			}
			_context.JobOpening.Update(jobOpening);

			await _context.SaveChangesAsync();

			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Error update JobOpening {JobOpening}", jobOpeningId);

			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	/// Remove a JobOpening and any related JobApplication.
	/// </summary>
	/// <param name="jobOpeningId">Id of the JobOpening you want to remove</param>
	/// <response code="200">If successfully removed</response>
	/// <response code="404">If there is no JobOpening with jobOpeningId</response>
	[HttpDelete("{jobOpeningId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult> DeleteJobOpening(int jobOpeningId)
	{
		try
		{
			var jobOpening = await _context.JobOpening.FindAsync(jobOpeningId);
			if (jobOpening == null)
			{
				return NotFound();
			}
			_context.JobOpening.Remove(jobOpening);
			await _context.SaveChangesAsync();

			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Error deleting JobOpening {JobOpening}", jobOpeningId);

			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	/// List the candidades for the jobOpeningId
	/// </summary>
	/// <param name="jobOpeningId">Id of the JobOpening</param>
	/// <returns>A list of Candidates</returns>
	/// <response code="200">Returns a list of the Candidates</response>
	[HttpGet("{jobOpeningId}/Candidates")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<CandidateDto>>> ListCandidateByJobOpening(int jobOpeningId)
	{
		try
		{
			var jobApplications = await _context.JobApplication
				.Where(j => j.JobOpeningId == jobOpeningId)
				.Include(j => j.Candidate)
				.ToListAsync();

			return Ok(jobApplications.Select(j => j.Candidate.ToDto()));
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Error fetching a list of Candidates");

			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	/// Apply for a jobOpeningId.
	/// </summary>
	/// <param name="jobOpeningId">Id of the JobOpening the Candidate is interested in</param>
	/// <param name="request">The Candidate information</param>
	/// <returns>The application info</returns>
	/// <response code="201">Returns the application info</response>
	/// <response code="409">If the candidate has applied for the jobOpeningId previously.</response>
	[HttpPost("{jobOpeningId}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public async Task<ActionResult<JobApplicationDto>> ApplyForJob(int jobOpeningId, CreateApplicationDto request)
	{
		try
		{
			var application = await _context.JobApplication.FindAsync(jobOpeningId, request.CandidateId);
			if (application != null)
			{
				return Conflict();
			}
			var jobOpening = await _context.JobOpening.FindAsync(jobOpeningId);
			if (jobOpening == null)
			{
				return NotFound();
			}
			var candidate = await _context.Candidates.FindAsync(request.CandidateId);
			if (candidate == null)
			{
				return NotFound();
			}

			var jobApplication = new JobApplication
			{
				JobOpeningId = jobOpeningId,
				CandidateId = request.CandidateId,
				JobOpening = jobOpening,
				Candidate = candidate,
			};

			await _context.JobApplication.AddAsync(jobApplication);
			await _context.SaveChangesAsync();

			return CreatedAtAction(
				nameof(JobApplicationController.GetJobApplication),
				"JobApplication",
				new { jobOpeningId = jobOpeningId, candidateId = request.CandidateId },
				jobApplication.ToDto());
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Could not create a new JobApplication");

			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}
}