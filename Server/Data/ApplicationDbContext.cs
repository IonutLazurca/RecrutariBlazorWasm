using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecrutariBlazorWasm.Shared.Entities;
using System;

namespace RecrutariBlazorWasm.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<Recruiter>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ApplicantProject> ApplicantsProjects { get; set; }
        public DbSet<LanguageQualification> LanguagesQualifications { get; set; }
        //public DbSet<Holiday> Holidays { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicantProject>().HasKey(x => new { x.ApplicantId, x.ProjectId });

            builder.Entity<ApplicantProject>()
                        .HasOne(x => x.Applicant).WithMany(y => y.ApplicantProject)
                        //.HasForeignKey(fk => fk.ApplicantId)
                        .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicantProject>()
                        .HasOne(x => x.Project).WithMany(y => y.ApplicantProject)
                        //.HasForeignKey(fk => fk.ProjectId)
                        .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(builder);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.LogTo(Console.WriteLine);
    }
}
