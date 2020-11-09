using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RSA316Infinito.BigRsaCrypto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using UniSA.Domain;
using UniSA.Services.RepositoryServices;
using UniSA.Services.StratisBlockChainServices;
using UniSA.Services.StratisBlockChainServices.Providers;
using UniSA.Services.StratisBlockChainServices.StratisApi;
using UniSA.Services.UnitOfWork;
using UniSAEmloyeeEmployerCertificationAndEngagement.App_Start;
using UniSAEmloyeeEmployerCertificationAndEngagement.Models;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Controllers
{
    [Authorize(Roles = "Employer,Administrator")]
    public class EmployerController : Controller
    {
        UnitOfWork _unitOfWork;
        private RoleManager<IdentityRole> _roleManager;
        private RepositoryEndPointServices _repositoryEndPointService;
        private ApplicationUserManager _userManager;

        public EmployerController(UnitOfWork unitOfWork, ApplicationUserManager userManager)
        {
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            _unitOfWork = unitOfWork;
            _unitOfWork.UniSADbContext = new UniSA.DataAccess.UniSADbContext();
            var stratisEndPoint = new StratisEndPointAdhocService();
            var moocMicroCredentialProvider = new MoocMicroCredentialProvider(new StratisEndPointAdhocService(), new EncryptDecrypt());
            var employerProvider = new EmployerOperations(stratisEndPoint, new EncryptDecrypt(), moocMicroCredentialProvider);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["StratisBlockChainBaseUrl"]);
            var stratisApiRequests = new UniSA.Services.StratisBlockChainServices.StratisApi.StratisApiFullfilRequestComponent(client); ;
            moocMicroCredentialProvider.StratisApiFullfilRequestComponent = stratisApiRequests;
            _repositoryEndPointService = new RepositoryEndPointServices(_unitOfWork, moocMicroCredentialProvider);
            _repositoryEndPointService.EmployerOperations = employerProvider;
            moocMicroCredentialProvider.RepositoryEndPointService = _repositoryEndPointService;
            employerProvider.RepositoryEndPointService = _repositoryEndPointService;
            employerProvider.StratisApiFullfilRequestComponent = stratisApiRequests;
            _userManager = userManager;
        }
        private List<SelectListItem> GetRoles()
        {
            return _roleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();
        }
        private List<SelectListItem> GetAddressIds()
        {
            return _unitOfWork.AddressRepository.GetAll().Select(a => new SelectListItem { Text = a.AddressLine1, Value = a.AddressId.ToString() }).ToList();
        }
        private List<SelectListItem> GetAccreditationBodyIds()
        {
            return _unitOfWork.AccreditationBodyRepository.GetAll().Select(a => new SelectListItem { Text = a.AccreditationBodyName, Value = a.AccreditationBodyId.ToString() }).ToList();
        }

        private List<SelectListItem> GetEndorsedBodyIds()
        {
            return _unitOfWork.EndorsementBodyRepository.GetAll().Select(a => new SelectListItem { Text = a.EndorsementBodyName, Value = a.EndorsementBodyId.ToString() }).ToList();
        }
        private List<SelectListItem> GetCandidateIds()
        {
            return _unitOfWork.CandidateRepository.GetAll().Select(a => new SelectListItem { Text = a.UserName, Value = a.CandidateId.ToString() }).ToList();
        }
        private List<SelectListItem> GetEmployerIds()
        {
            return _unitOfWork.EmployerRepository.GetAll().Select(a => new SelectListItem { Text = a.EmployerName, Value = a.EmployerId.ToString() }).ToList();
        }
        private List<SelectListItem> GetJobIds()
        {
            return _unitOfWork.JobRepository.GetAll().Select(a => new SelectListItem { Text = a.JobTitle, Value = a.JobId.ToString() }).ToList();
        }
        private List<SelectListItem> GetJobIdsForEmployer(int employerId)
        {
            return _unitOfWork.JobRepository.GetAll().Where(j => j.EmployerId == employerId).Select(a => new SelectListItem { Text = a.JobTitle, Value = a.JobId.ToString() }).ToList();
        }
        private List<SelectListItem> GetMicroCredentialIds()
        {
            return _unitOfWork.MicroCredentialRepository.GetAll().Select(a => new SelectListItem { Text = a.MicroCredentialName, Value = a.MicroCredentialId.ToString() }).ToList();
        }
        private List<SelectListItem> GetGovernmentIds()
        {
            return _unitOfWork.GorvernmentRepository.GetAll().Select(a => new SelectListItem { Text = a.GovernmentDepartmentName, Value = a.DepartmentAddressId.ToString() }).ToList();
        }
        private List<SelectListItem> GetCandidateMicroCredentialCourseIds()
        {
            return _unitOfWork.CandidateMicroCredentialCourseRepository.GetAll().Select(a => new SelectListItem { Text = "[" + a.MicroCredential.MicroCredentialName + "@" + a.Candidate.EmailAddress + "]", Value = a.CandidateMicroCredentialCourseId.ToString() }).ToList();
        }
        private List<SelectListItem> GetMoocProviderIds()
        {
            return _unitOfWork.MoocProviderRepository.GetAll().Select(a => new SelectListItem { Text = a.MoocProviderName, Value = a.MoocProviderId.ToString() }).ToList();
        }
        private List<SelectListItem> GetRecrutimentAgentIds()
        {
            return _unitOfWork.RecruitmentAgencyRepository.GetAll().Select(a => new SelectListItem { Text = a.RecruitmentAgencyName, Value = a.RecruitmentAgencyId.ToString() }).ToList();
        }
        private List<SelectListItem> GetMicroCredentialBadgeIds()
        {
            return _unitOfWork.CandidatetMicroCredentialBadgesRepository.GetAll().Select(a => new SelectListItem { Text = a.MicroCredentialBadges, Value = a.MicroCredentialBadgeId.ToString() }).ToList();
        }
        // GET: Employer
        [HttpGet]
        public ActionResult Index(string AddminOtherRole)
        {
            ViewBag.AddminOtherRole = AddminOtherRole;
            return View();
        }

        [HttpGet]
        public ActionResult EmployerCreateJob()
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            return View();
        }

        [HttpPost]
        public ActionResult EmployerCreateJob(JobViewModel jobViewModel)
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            if (ModelState.IsValid)
            {
                var job = AutoMapperConfig.Configure().Map(jobViewModel, typeof(JobViewModel), typeof(Job)) as Job;
                _repositoryEndPointService.EmployerCreateJob(job);
                return View("Success");
            }
            return View(jobViewModel);
        }
        [HttpGet]
        public ActionResult SelectEmployerJob()
        {
            var initJobModel = new SelectDeleteJobViewModel { NumberOfPositions = 1, IsActive = false };
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            var employer = _repositoryEndPointService.GetEmployerByEmail(User.Identity.Name);
            if (employer == null) return View("Failed");
            var emptySelectJobIds = new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            ViewBag.JobsIdList = GetJobIdsForEmployer(employer.EmployerId) ?? emptySelectJobIds;
            return View(initJobModel);
        }

        [HttpPost]
        public ActionResult SelectEmployerJob(SelectDeleteJobViewModel jobViewModel)
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            var employer = _repositoryEndPointService.GetEmployerByEmail(User.Identity.Name);
            if (employer == null) return View("Failed");
            var emptySelectJobIds = new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            ViewBag.JobsIdList = GetJobIdsForEmployer(employer.EmployerId) ?? emptySelectJobIds;
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                var actJob = _repositoryEndPointService.GetJobById(jobViewModel.JobId);
                var jobSelectedViewModel = AutoMapperConfig.Configure().Map(actJob, typeof(Job), typeof(SelectDeleteJobViewModel)) as SelectDeleteJobViewModel;

                return View(jobSelectedViewModel);
            }
            return View(jobViewModel);
        }
        [HttpGet]
        public ActionResult UpdateEmployerJob()
        {
            var initJobModel = new JobViewModel { NumberOfPositions = 1, IsActive = false };
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            var employer = _repositoryEndPointService.GetEmployerByEmail(User.Identity.Name);
            if (employer == null) return View("Failed");
            var emptySelectJobIds = new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            ViewBag.JobsIdList = GetJobIdsForEmployer(employer.EmployerId) ?? emptySelectJobIds;
            return View(initJobModel);
        }

        [HttpPost]
        public ActionResult UpdateEmployerJob(JobViewModel jobViewModel)
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            var employer = _repositoryEndPointService.GetEmployerByEmail(User.Identity.Name);
            if (employer == null) return View("Failed");
            var emptySelectJobIds = new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            ViewBag.JobsIdList = GetJobIdsForEmployer(employer.EmployerId) ?? emptySelectJobIds;
            if (ModelState.IsValid)
            {
                var job = AutoMapperConfig.Configure().Map(jobViewModel, typeof(JobViewModel), typeof(Job)) as Job;
                _repositoryEndPointService.EmployerUpdateJob(job);
                return View("Success");
            }
            return View(jobViewModel);
        }

        [HttpGet]
        public ActionResult GetJobsByEmployerId(int id)
        {
            ViewBag.EmployerIdList = GetEmployerIds();
            var jobs = _repositoryEndPointService.GetJobsByEmployerId(id);
            var jobsViewModel = AutoMapperConfig.Configure().Map(jobs, typeof(Job[]), typeof(JobViewModel[])) as JobViewModel[];
            return View(jobsViewModel);
        }
        [HttpPost]
        public ActionResult DeleteJob(int employerId, int jobId)
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            _repositoryEndPointService.DeleteJobs(employerId, jobId);
            return View("Success");
        }
        [HttpGet]
        public string GetMicroCredentialCertificateUrlById(int candidateId, int microCredentialId)
        {
            UserMicroCredentialBadges microCredentialBadges = _repositoryEndPointService.GetUserMicroCredentialBadgesById(candidateId, microCredentialId);
            return "/images/Certificates/" + microCredentialBadges.Username + "_" + microCredentialBadges.MicroCredentialBadges + ".jpg";
        }
        [HttpGet]
        public ActionResult ValidateCandidateMicroCredential()
        {
            ViewBag.MicroCredentialName = string.Empty;
            ViewBag.CertificateImageUrl = new string[] { string.Empty };
            ViewBag.MicroCredentialBadgesIdList = GetMicroCredentialBadgeIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.CandidateIdList = GetCandidateIds();
            return View();
        }
        [HttpPost]
        public ActionResult ValidateCandidateMicroCredential(ValidateUserMicroCredentialBadges userMicroCredentialViewModel)
        {
            ViewBag.MicroCredentialBadgesIdList = GetMicroCredentialBadgeIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.CandidateIdList = GetCandidateIds();

            if (ModelState.IsValid)
            {
                var userMicroCredential = AutoMapperConfig.Configure().Map(userMicroCredentialViewModel, typeof(ValidateUserMicroCredentialBadges), typeof(UserMicroCredentialBadges)) as UserMicroCredentialBadges;
                _repositoryEndPointService.VerifyUserBadges(userMicroCredential);
                return View("Success");
            }
            return View(userMicroCredentialViewModel);
        }

        [HttpGet]
        //List Of Employers And Link to their Jobs
        public ActionResult EmployerJobList()
        {
            var employerList = new List<EmployerJobsViewModel>();

            var employersJobs = (from emp in _unitOfWork.EmployerRepository.GetAll()
                                 join j in _unitOfWork.JobRepository.GetAll()
                                 on emp.EmployerId equals j.EmployerId into empJobs
                                 group empJobs by new { emp.EmployerName, emp.EmployerId, emp.Sector } into empJobGroups
                                 select new
                                 {
                                     Employer = new EmployerViewModel
                                     {
                                         EmployerId = empJobGroups.Key.EmployerId,
                                         EmployerName = empJobGroups.Key.EmployerName,
                                         Sector = empJobGroups.Key.Sector,
                                     },
                                     Jobs = (from job in empJobGroups
                                             select job
                                           )
                                 }).ToList();

            foreach (var empJobs in employersJobs)
            {
                var em = empJobs.Employer;
                var jLs = new List<JobViewModel>();
                foreach (var job in empJobs.Jobs.ToList().FirstOrDefault())
                {
                    var jobViewModel = new JobViewModel { JobTitle = job.JobTitle, JobId = job.JobId, JobCode = job.JobCode, JobDescription = job.JobDescription };
                    if (!jLs.Contains(jobViewModel))
                    {
                        jLs.Add(jobViewModel);
                    }
                }
                employerList.Add(new EmployerJobsViewModel { Employer = em, Jobs = jLs.ToArray() });
            }
            return View(employerList);
        }
    }
}
