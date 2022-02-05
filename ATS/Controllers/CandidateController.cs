using ATS.DTOs;
using ATS.DTOs.Candidates;
using ATS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ATS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CandidateController : ControllerBase
{
	private readonly ILogger<CandidateController> _logger;
	private readonly ApplicationDbContext _context;

	public CandidateController(ILogger<CandidateController> logger, ApplicationDbContext context)
	{
		_logger = logger;
		_context = context;
	}

	/// <summary>
	/// Retrieve a Candidate.
	/// </summary>
	/// <param name="candidateId">Id of the Candidate you want to retrieve</param>
	/// <param name="full">True if you want to fetch the CV in this request</param>
	/// <returns>The Candidate</returns>
	/// <response code="200">The Candidate</response>
	/// <response code="404">If there is no Candidate with candidateId</response>
	[HttpGet("{candidateId}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<CandidateDto>> GetCandidate(int candidateId, bool full = false)
	{
		try
		{
			Candidate candidate = null;
			if (full)
			{
				candidate = await _context.Candidates.Include(c => c.CandidateCurriculumVitae)
					.FirstOrDefaultAsync(c => c.CandidateId == candidateId);
			}
			else
			{
				candidate = await _context.Candidates.FindAsync(candidateId);
			}

			if (candidate == null)
			{
				return NotFound();
			}

			return Ok(candidate.ToDto());
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Error fetching Candidate {CandidateId}", candidateId);

			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	/// Create a new Candidate.
	/// </summary>
	/// <param name="request">Candidate details</param>
	/// <returns>The new Candidate</returns>
	/// <response code="200">The new Candidade</response>
	[HttpPost]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public async Task<ActionResult<CandidateDto>> CreateCandidate([FromForm] CreateCandidateDto request)
	{
		try
		{
			var candidate = new Candidate { Name = request.Name, };

			if (request.File != null)
			{
				using var memoryStream = new MemoryStream();

				await request.File.CopyToAsync(memoryStream);

				candidate.CandidateCurriculumVitae = new CandidateCurriculumVitae
				{
					File = memoryStream.ToArray(),
					FileName = request.File.FileName,
				};
			}
			await _context.Candidates.AddAsync(candidate);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetCandidate), new { candidateId = candidate.CandidateId }, candidate.ToDto());
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Could not create a new Candidate");

			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	/// Update a Candidate.
	/// </summary>
	/// <param name="candidateId">Id of the Candidate to be updated</param>
	/// <param name="request">Candidate details</param>
	/// <response code="200">If successfully updated</response>
	/// <response code="404">If there is no Candidate with candidateId</response>
	[HttpPut("{candidateId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult> UpdateCandidate(int candidateId, UpdateCandidateDto request)
	{
		try
		{
			var candidate = await _context.Candidates.FindAsync(candidateId);
			if (candidate == null)
			{
				return NotFound();
			}

			if (request.Name != null)
			{
				candidate.Name = request.Name;
			}
			_context.Candidates.Update(candidate);

			await _context.SaveChangesAsync();

			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Error update Candidate {CandidateId}", candidateId);

			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	/// Remove a Candidate and any related JobApplication.
	/// </summary>
	/// <param name="candidateId">Id of the Candidate you want to remove</param>
	/// <response code="200">If successfully removed the Candidate</response>
	/// <response code="404">If there is no Candidate with candidateId</response>
	[HttpDelete("{candidateId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult> DeleteCandidate(int candidateId)
	{
		try
		{
			var candidate = await _context.Candidates.FindAsync(candidateId);
			if (candidate == null)
			{
				return NotFound();
			}
			_context.Candidates.Remove(candidate);
			await _context.SaveChangesAsync();

			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Error deleting Candidate {CandidateId}", candidateId);

			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	/// <summary>
	/// Retrieve a Candidate CV.
	/// </summary>
	/// <param name="candidateId">Id of the Candidate you want to retrieve the CV</param>
	/// <returns>The Candidate CV</returns>
	/// <response code="200">The Candidate CV</response>
	/// <response code="404">If there is no Candidate with candidateId</response>
	[HttpGet("{candidateId}/cv")]
	[Produces("application/octet-stream")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<byte[]>> GetCandidateCV(int candidateId)
	{
		try
		{
			var cv = await _context.CandidateCurriculumVitae.FindAsync(candidateId);
			if (cv == null)
			{
				return NotFound();
			}
			var stream = new MemoryStream(cv.File);

			return new FileStreamResult(stream, "application/octet-stream")
			{
				FileDownloadName = cv.FileName
			};
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Error fetching CurriculumVitae for Candidate {CandidateId}", candidateId);

			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}
}