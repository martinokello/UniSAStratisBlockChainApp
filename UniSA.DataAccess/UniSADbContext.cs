using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using UniSA.Domain;

namespace UniSA.DataAccess
{
    public class UniSADbContext:DbContext
    {
        public UniSADbContext():base("UniSADbConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<UniSADbContext>());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<MicroCredential> MicroCredentials { get; set; }
        public DbSet<MoocProvider> MoocProviders { get; set; }
        public DbSet<RecruitmentAgency> RecruitmentAgencies { get; set; }
        public DbSet<Government> Governments { get; set; }
        public DbSet<CandidateMicroCredentialCourse> CandidateMicroCredentialCourses { get; set; }
        public DbSet<AccreditationBody> AccreditationBodies { get; set; }
        public DbSet<EndorsementBody> EndorsementBodies { get; set; }
        public DbSet<CandidateJobApplication> CandidateJobApplications { get; set; }
        public DbSet<UserMicroCredentialBadges> UserMicroCredentialBadgess { get; set; }
        public DbSet<StratisAccount> StratisAccounts { get; set; }
    }
}
