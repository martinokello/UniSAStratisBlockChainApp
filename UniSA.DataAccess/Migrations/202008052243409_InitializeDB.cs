namespace UniSA.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializeDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccreditationBodies",
                c => new
                    {
                        AccreditationBodyId = c.Int(nullable: false, identity: true),
                        AccreditationBodyName = c.String(),
                        EmailAddress = c.String(),
                        ContactNumber = c.String(),
                        AddressId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AccreditationBodyId)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        Town = c.String(),
                        Country = c.String(),
                        PostCode = c.String(),
                    })
                .PrimaryKey(t => t.AddressId);
            
            CreateTable(
                "dbo.CandidateJobApplications",
                c => new
                    {
                        CandidateJobApplicationId = c.Int(nullable: false, identity: true),
                        JobId = c.Int(nullable: false),
                        CandidateId = c.Int(nullable: false),
                        EmployerId = c.Int(nullable: false),
                        IsCertified = c.Boolean(nullable: false),
                        IsFullyPaidForCourse = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CandidateJobApplicationId)
                .ForeignKey("dbo.Candidates", t => t.CandidateId)
                .ForeignKey("dbo.Employers", t => t.EmployerId)
                .ForeignKey("dbo.Jobs", t => t.JobId)
                .Index(t => t.JobId)
                .Index(t => t.CandidateId)
                .Index(t => t.EmployerId);
            
            CreateTable(
                "dbo.Candidates",
                c => new
                    {
                        CandidateId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        EmailAddress = c.String(),
                        Password = c.String(),
                        AddressId = c.Int(nullable: false),
                        ContactNumber = c.String(),
                        HighestQualification = c.String(),
                        AppliedJobsId = c.Int(nullable: false),
                        DigitalBadgeEarned = c.String(),
                        MicroCredentialEnrolledId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CandidateId)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .ForeignKey("dbo.Jobs", t => t.AppliedJobsId)
                .ForeignKey("dbo.MicroCredentials", t => t.MicroCredentialEnrolledId)
                .Index(t => t.AddressId)
                .Index(t => t.AppliedJobsId)
                .Index(t => t.MicroCredentialEnrolledId);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        JobCode = c.String(),
                        JobTitle = c.String(),
                        JobDescription = c.String(),
                        NumberOfPositions = c.Int(nullable: false),
                        QualificationsRequired = c.Boolean(nullable: false),
                        MicroCredentialRequired = c.Boolean(nullable: false),
                        EmployerId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobId)
                .ForeignKey("dbo.Employers", t => t.EmployerId)
                .Index(t => t.EmployerId);
            
            CreateTable(
                "dbo.Employers",
                c => new
                    {
                        EmployerId = c.Int(nullable: false, identity: true),
                        EmployerName = c.String(),
                        AddressId = c.Int(nullable: false),
                        ContactPerson = c.String(),
                        ContactNumber = c.String(),
                        ContactEmailAddress = c.String(),
                        Sector = c.String(),
                    })
                .PrimaryKey(t => t.EmployerId)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.MicroCredentials",
                c => new
                    {
                        MicroCredentialId = c.Int(nullable: false, identity: true),
                        MicroCredentialCode = c.String(),
                        MicroCredentialName = c.String(),
                        MicroCredentialDescription = c.String(),
                        NumberOfCredits = c.Int(nullable: false),
                        AccreditationBodyId = c.Int(nullable: false),
                        Fee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CertificateFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DurationStart = c.DateTime(nullable: false),
                        DurationEnd = c.DateTime(nullable: false),
                        DigitalBadge = c.String(),
                        IsAccredited = c.Boolean(nullable: false),
                        IsEndorsed = c.Boolean(nullable: false),
                        EndorsementBodyId = c.Int(nullable: false),
                        JobId = c.Int(nullable: false),
                        Candidate_CandidateId = c.Int(),
                    })
                .PrimaryKey(t => t.MicroCredentialId)
                //.ForeignKey("dbo.AccreditationBodies", t => t.AccreditationBodyId)
                //.ForeignKey("dbo.EndorsementBodies", t => t.EndorsementBodyId)
                //.ForeignKey("dbo.Jobs", t => t.JobId)
                //.ForeignKey("dbo.Candidates", t => t.Candidate_CandidateId)
                .Index(t => t.AccreditationBodyId)
                .Index(t => t.EndorsementBodyId)
                .Index(t => t.JobId)
                .Index(t => t.Candidate_CandidateId);
            
            CreateTable(
                "dbo.EndorsementBodies",
                c => new
                    {
                        EndorsementBodyId = c.Int(nullable: false, identity: true),
                        EndorsementBodyName = c.String(),
                        EmailAddress = c.String(),
                        ContactNumber = c.String(),
                        AddressId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EndorsementBodyId)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.CandidateMicroCredentialCourses",
                c => new
                    {
                        CandidateMicroCredentialCourseId = c.Int(nullable: false, identity: true),
                        CandidateId = c.Int(nullable: false),
                        MicroCredentialId = c.Int(nullable: false),
                        HasPassed = c.Boolean(nullable: false),
                        HashOfMine = c.String(),
                        MineBlockContent = c.String(),
                        MineAddressTip = c.String(),
                    })
                .PrimaryKey(t => t.CandidateMicroCredentialCourseId)
                .ForeignKey("dbo.Candidates", t => t.CandidateId)
                .ForeignKey("dbo.MicroCredentials", t => t.MicroCredentialId)
                .Index(t => t.CandidateId)
                .Index(t => t.MicroCredentialId);
            
            CreateTable(
                "dbo.Governments",
                c => new
                    {
                        GovernmentDepartmentId = c.Int(nullable: false, identity: true),
                        Country = c.String(),
                        GovernmentDepartmentName = c.String(),
                        DepartmentAddressId = c.Int(nullable: false),
                        GovernmentDepartmentContactNumber = c.String(),
                        ContactName = c.String(),
                        ContactNumber = c.String(),
                        ContactEmailAddress = c.String(),
                    })
                .PrimaryKey(t => t.GovernmentDepartmentId)
                .ForeignKey("dbo.Addresses", t => t.DepartmentAddressId)
                .Index(t => t.DepartmentAddressId);
            
            CreateTable(
                "dbo.MoocProviders",
                c => new
                    {
                        MoocProviderId = c.Int(nullable: false, identity: true),
                        MoocProviderNumber = c.String(),
                        MoocProviderContactNumber = c.String(),
                        MoocProviderName = c.String(),
                        AddressId = c.Int(nullable: false),
                        EmailAddress = c.String(),
                        MoocPublicKey = c.Binary(),
                        MoocPrivateKey = c.Binary(),
                        MoocModulus = c.Binary(),
                    })
                .PrimaryKey(t => t.MoocProviderId)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.RecruitmentAgencies",
                c => new
                    {
                        RecruitmentAgencyId = c.Int(nullable: false, identity: true),
                        RecruitmentAgencyName = c.String(),
                        AddressId = c.Int(nullable: false),
                        ContactPerson = c.String(),
                        ContactNumber = c.String(),
                        ContactEmailAddress = c.String(),
                        JobAdvertisedId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RecruitmentAgencyId)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .ForeignKey("dbo.Jobs", t => t.JobAdvertisedId)
                .Index(t => t.AddressId)
                .Index(t => t.JobAdvertisedId);
            
            CreateTable(
                "dbo.UserMicroCredentialBadges",
                c => new
                    {
                        MicroCredentialBadgeId = c.Int(nullable: false, identity: true),
                        CandidateId = c.Int(nullable: false),
                        MicroCredentialId = c.Int(nullable: false),
                        Username = c.String(),
                        MicroCredentialBadges = c.String(),
                    })
                .PrimaryKey(t => t.MicroCredentialBadgeId)
                .ForeignKey("dbo.Candidates", t => t.CandidateId)
                .ForeignKey("dbo.MicroCredentials", t => t.MicroCredentialId)
                .Index(t => t.CandidateId)
                .Index(t => t.MicroCredentialId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserMicroCredentialBadges", "MicroCredentialId", "dbo.MicroCredentials");
            DropForeignKey("dbo.UserMicroCredentialBadges", "CandidateId", "dbo.Candidates");
            DropForeignKey("dbo.RecruitmentAgencies", "JobAdvertisedId", "dbo.Jobs");
            DropForeignKey("dbo.RecruitmentAgencies", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.MoocProviders", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Governments", "DepartmentAddressId", "dbo.Addresses");
            DropForeignKey("dbo.CandidateMicroCredentialCourses", "MicroCredentialId", "dbo.MicroCredentials");
            DropForeignKey("dbo.CandidateMicroCredentialCourses", "CandidateId", "dbo.Candidates");
            DropForeignKey("dbo.CandidateJobApplications", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.CandidateJobApplications", "EmployerId", "dbo.Employers");
            DropForeignKey("dbo.CandidateJobApplications", "CandidateId", "dbo.Candidates");
            DropForeignKey("dbo.MicroCredentials", "Candidate_CandidateId", "dbo.Candidates");
            DropForeignKey("dbo.Candidates", "MicroCredentialEnrolledId", "dbo.MicroCredentials");
            DropForeignKey("dbo.MicroCredentials", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.MicroCredentials", "EndorsementBodyId", "dbo.EndorsementBodies");
            DropForeignKey("dbo.EndorsementBodies", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.MicroCredentials", "AccreditationBodyId", "dbo.AccreditationBodies");
            DropForeignKey("dbo.Candidates", "AppliedJobsId", "dbo.Jobs");
            DropForeignKey("dbo.Jobs", "EmployerId", "dbo.Employers");
            DropForeignKey("dbo.Employers", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Candidates", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.AccreditationBodies", "AddressId", "dbo.Addresses");
            DropIndex("dbo.UserMicroCredentialBadges", new[] { "MicroCredentialId" });
            DropIndex("dbo.UserMicroCredentialBadges", new[] { "CandidateId" });
            DropIndex("dbo.RecruitmentAgencies", new[] { "JobAdvertisedId" });
            DropIndex("dbo.RecruitmentAgencies", new[] { "AddressId" });
            DropIndex("dbo.MoocProviders", new[] { "AddressId" });
            DropIndex("dbo.Governments", new[] { "DepartmentAddressId" });
            DropIndex("dbo.CandidateMicroCredentialCourses", new[] { "MicroCredentialId" });
            DropIndex("dbo.CandidateMicroCredentialCourses", new[] { "CandidateId" });
            DropIndex("dbo.EndorsementBodies", new[] { "AddressId" });
            DropIndex("dbo.MicroCredentials", new[] { "Candidate_CandidateId" });
            DropIndex("dbo.MicroCredentials", new[] { "JobId" });
            DropIndex("dbo.MicroCredentials", new[] { "EndorsementBodyId" });
            DropIndex("dbo.MicroCredentials", new[] { "AccreditationBodyId" });
            DropIndex("dbo.Employers", new[] { "AddressId" });
            DropIndex("dbo.Jobs", new[] { "EmployerId" });
            DropIndex("dbo.Candidates", new[] { "MicroCredentialEnrolledId" });
            DropIndex("dbo.Candidates", new[] { "AppliedJobsId" });
            DropIndex("dbo.Candidates", new[] { "AddressId" });
            DropIndex("dbo.CandidateJobApplications", new[] { "EmployerId" });
            DropIndex("dbo.CandidateJobApplications", new[] { "CandidateId" });
            DropIndex("dbo.CandidateJobApplications", new[] { "JobId" });
            DropIndex("dbo.AccreditationBodies", new[] { "AddressId" });
            DropTable("dbo.UserMicroCredentialBadges");
            DropTable("dbo.RecruitmentAgencies");
            DropTable("dbo.MoocProviders");
            DropTable("dbo.Governments");
            DropTable("dbo.CandidateMicroCredentialCourses");
            DropTable("dbo.EndorsementBodies");
            DropTable("dbo.MicroCredentials");
            DropTable("dbo.Employers");
            DropTable("dbo.Jobs");
            DropTable("dbo.Candidates");
            DropTable("dbo.CandidateJobApplications");
            DropTable("dbo.Addresses");
            DropTable("dbo.AccreditationBodies");
        }
    }
}
