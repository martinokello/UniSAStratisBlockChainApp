using UniSA.DataAccess;
using UniSA.Services.UnitOfWork.Interfaces;
using UniSA.DataAccess.Concretes;

namespace UniSA.Services.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private AddressRepository _addressRepository;
        private CandidateRepository _candidateRepository;
        private EmployerRepository _employerRepository;
        private GorvernmentRepository _gorvernmentRepository;
        private JobRepository _jobRepository;
        private MicroCredentialRepository _microCredentialRepository;
        private MoocProviderRepository _moocProviderRepository;
        private RecruitmentAgencyRepository _recruitmentAgencyRepository;
        private AccreditationBodyRepository _accreditationBodyRepository;
        private CandidateMicroCredentialCourseRepository _candidateMicroCredentialCourseRepository;
        private EndorsementBodyRepository _endorsementBodyRepository;
        private CandidateJobApplicationRepository _candidateJobApplicationRepository;
        private CandidatetMicroCredentialBadgesRepository _candidatetMicroCredentialBadgesRepository;
        private StratisAccountRepository _stratisAccountRepository;

        private UniSADbContext _unisaDbContext;
        public AddressRepository AddressRepository { get { return _addressRepository; } }
        public CandidateRepository CandidateRepository { get { return _candidateRepository; } }
        public EmployerRepository EmployerRepository { get { return _employerRepository; } }
        public GorvernmentRepository GorvernmentRepository { get { return _gorvernmentRepository; } }
        public JobRepository JobRepository { get { return _jobRepository; } }
        public MicroCredentialRepository MicroCredentialRepository { get { return _microCredentialRepository; } }
        public MoocProviderRepository MoocProviderRepository { get { return _moocProviderRepository; } }
        public RecruitmentAgencyRepository RecruitmentAgencyRepository { get { return _recruitmentAgencyRepository; } }
        public AccreditationBodyRepository AccreditationBodyRepository { get { return _accreditationBodyRepository; } }
        public CandidateMicroCredentialCourseRepository CandidateMicroCredentialCourseRepository { get { return _candidateMicroCredentialCourseRepository; } }
        public EndorsementBodyRepository EndorsementBodyRepository { get { return _endorsementBodyRepository; } }
        public CandidateJobApplicationRepository CandidateJobApplicationRepository { get { return _candidateJobApplicationRepository; } }
        public CandidatetMicroCredentialBadgesRepository CandidatetMicroCredentialBadgesRepository { get { return _candidatetMicroCredentialBadgesRepository; } }
        public StratisAccountRepository StratisAccountRepository { get { return _stratisAccountRepository; } }
        
        public UnitOfWork(AddressRepository addressRepository, CandidateRepository candidateRepository,EmployerRepository employerRepository,
            GorvernmentRepository gorvernmentRepository, JobRepository jobRepository, MicroCredentialRepository microCredentialRepository, MoocProviderRepository moocProviderRepository,
             RecruitmentAgencyRepository recruitmentAgencyRepository, AccreditationBodyRepository accreditationBodyRepository, CandidateMicroCredentialCourseRepository candidateMicroCredentialCourseRepository, 
             EndorsementBodyRepository endorsementBodyRepository, CandidateJobApplicationRepository candidateJobApplicationRepository, CandidatetMicroCredentialBadgesRepository candidatetMicroCredentialBadgesRepository,
             StratisAccountRepository stratisAccountRepository)
        {
            _addressRepository = addressRepository;

            _candidateRepository = candidateRepository;

            _employerRepository = employerRepository;

            _gorvernmentRepository = gorvernmentRepository;

            _jobRepository = jobRepository;

            _microCredentialRepository = microCredentialRepository;

            _moocProviderRepository = moocProviderRepository;

            _recruitmentAgencyRepository = recruitmentAgencyRepository;

            _accreditationBodyRepository = accreditationBodyRepository;

            _candidateMicroCredentialCourseRepository = candidateMicroCredentialCourseRepository;

            _endorsementBodyRepository = endorsementBodyRepository;

            _candidateJobApplicationRepository = candidateJobApplicationRepository;

            _candidatetMicroCredentialBadgesRepository = candidatetMicroCredentialBadgesRepository;

            _stratisAccountRepository = stratisAccountRepository;
        }
        public void InitialiseRepositoryDbContexts()
        {
            _addressRepository.UniSADbContextInstance = _unisaDbContext;
            _addressRepository.DbContextUniSADbContext = _unisaDbContext;

            _candidateRepository.UniSADbContextInstance = _unisaDbContext;
            _candidateRepository.DbContextUniSADbContext = _unisaDbContext;
            _employerRepository.UniSADbContextInstance = _unisaDbContext;
            _employerRepository.DbContextUniSADbContext = _unisaDbContext;
            _gorvernmentRepository.UniSADbContextInstance = _unisaDbContext;
            _gorvernmentRepository.DbContextUniSADbContext = _unisaDbContext;
            _jobRepository.UniSADbContextInstance = _unisaDbContext;
            _jobRepository.DbContextUniSADbContext = _unisaDbContext;
            _microCredentialRepository.UniSADbContextInstance = _unisaDbContext;
            _microCredentialRepository.DbContextUniSADbContext = _unisaDbContext;
            _moocProviderRepository.UniSADbContextInstance = _unisaDbContext;
            _moocProviderRepository.DbContextUniSADbContext = _unisaDbContext;
            _recruitmentAgencyRepository.UniSADbContextInstance = _unisaDbContext;
            _recruitmentAgencyRepository.DbContextUniSADbContext = _unisaDbContext;
            _candidateMicroCredentialCourseRepository.UniSADbContextInstance = _unisaDbContext;
            _candidateMicroCredentialCourseRepository.DbContextUniSADbContext = _unisaDbContext;
            _accreditationBodyRepository.UniSADbContextInstance = _unisaDbContext;
            _accreditationBodyRepository.DbContextUniSADbContext = _unisaDbContext;
            _endorsementBodyRepository.UniSADbContextInstance = _unisaDbContext;
            _endorsementBodyRepository.DbContextUniSADbContext = _unisaDbContext;
            _candidateJobApplicationRepository.UniSADbContextInstance = _unisaDbContext;
            _candidateJobApplicationRepository.DbContextUniSADbContext = _unisaDbContext;
            _candidatetMicroCredentialBadgesRepository.UniSADbContextInstance = _unisaDbContext;
            _candidatetMicroCredentialBadgesRepository.DbContextUniSADbContext = _unisaDbContext;
            _stratisAccountRepository.UniSADbContextInstance = _unisaDbContext;
            _stratisAccountRepository.DbContextUniSADbContext = _unisaDbContext;
        }
        public UniSADbContext UniSADbContext
        { 
            get { return _unisaDbContext; }
            set { _unisaDbContext = value;
                this.InitialiseRepositoryDbContexts();
            }
        }

        public void SaveChanges()
        {
            UniSADbContext.SaveChanges();
        }
    }
}
