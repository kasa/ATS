using Microsoft.EntityFrameworkCore;

namespace ATS.Models;

public sealed class ApplicationDbContext : DbContext
{
	public DbSet<Candidate> Candidates => Set<Candidate>();
	public DbSet<CandidateCurriculumVitae> CandidateCurriculumVitae => Set<CandidateCurriculumVitae>();
	public DbSet<JobOpening> JobOpening => Set<JobOpening>();
	public DbSet<JobApplication> JobApplication => Set<JobApplication>();

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Candidate>()
			.HasKey(c => c.CandidateId);

		modelBuilder.Entity<Candidate>()
			.Property(c => c.CandidateId)
			.ValueGeneratedOnAdd();

		modelBuilder.Entity<CandidateCurriculumVitae>()
			.HasKey(c => c.CandidateId);

		modelBuilder.Entity<CandidateCurriculumVitae>()
			.HasOne(c => c.Candidate)
			.WithOne(c => c.CandidateCurriculumVitae);

		modelBuilder.Entity<JobOpening>()
			.HasKey(c => c.JobOpeningId);

		modelBuilder.Entity<JobOpening>()
			.Property(c => c.JobOpeningId)
			.ValueGeneratedOnAdd();

		modelBuilder.Entity<JobApplication>()
			.HasKey(c => new { c.JobOpeningId, c.CandidateId });

		modelBuilder.Entity<JobApplication>()
			.Property(c => c.ApplicationDate)
			.HasDefaultValueSql("date('now')");

		modelBuilder.Entity<JobApplication>()
			.HasOne(c => c.JobOpening)
			.WithMany(c => c.JobCandidates);

		modelBuilder.Entity<JobApplication>()
			.HasOne(c => c.Candidate)
			.WithMany(c => c.JobApplications);
	}
}