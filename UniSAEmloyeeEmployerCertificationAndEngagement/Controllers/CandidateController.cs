using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RSA316Infinito.BigRsaCrypto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using UniSA.Domain;
using UniSA.Services.RepositoryServices;
using UniSA.Services.StratisBlockChainServices;
using UniSA.Services.StratisBlockChainServices.Providers;
using UniSA.Services.UnitOfWork;
using UniSAEmloyeeEmployerCertificationAndEngagement.App_Start;
using UniSAEmloyeeEmployerCertificationAndEngagement.Models;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Controllers
{
    [Authorize(Roles = "Candidate,Administrator")]
    public class CandidateController : Controller
    {
        public UnitOfWork _unitOfWork;
        private RoleManager<IdentityRole> _roleManager;
        private RepositoryEndPointServices _repositoryEndPointService;

        public CandidateController(UnitOfWork unitOfWork)
        {
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            _unitOfWork = unitOfWork;
            _unitOfWork.UniSADbContext = new UniSA.DataAccess.UniSADbContext();
            var stratisEndPoint = new StratisEndPointAdhocService();
            var moocMicroCredentialProvider = new MoocMicroCredentialProvider(new StratisEndPointAdhocService(), new EncryptDecrypt());

            var employeeProvider = new EmployeeOperations(stratisEndPoint, new EncryptDecrypt(), moocMicroCredentialProvider);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["StratisBlockChainBaseUrl"]);
            var stratisApiRequests = new UniSA.Services.StratisBlockChainServices.StratisApi.StratisApiFullfilRequestComponent(client); ;
            moocMicroCredentialProvider.StratisApiFullfilRequestComponent = stratisApiRequests;
            _repositoryEndPointService = new RepositoryEndPointServices(_unitOfWork, moocMicroCredentialProvider); 
            _repositoryEndPointService.EmployeeOperations = employeeProvider;
            moocMicroCredentialProvider.RepositoryEndPointService = _repositoryEndPointService;
            employeeProvider.RepositoryEndPointService = _repositoryEndPointService;
            employeeProvider.StratisApiFullfilRequestComponent = stratisApiRequests;
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
        private List<SelectListItem> GetCandidateMicroCredentialBadgesIds()
        {
            return _unitOfWork.CandidatetMicroCredentialBadgesRepository.GetAll().Select(a => new SelectListItem { Text = a.MicroCredentialBadges, Value = a.MicroCredentialBadgeId.ToString() }).ToList();
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
        [HttpGet]
        public ActionResult ValidateUserMicroCredential()
        {
            ViewBag.MicroCredentialBadgesIdList = GetMicroCredentialBadgeIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.CandidateIdList = GetCandidateIds();
            return View();
        }
        [HttpPost]
        public ActionResult ValidateUserMicroCredential(ValidateUserMicroCredentialBadges userMicroCredentialViewModel)
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
        // GET: Candidate
        [HttpGet]
        public ActionResult Index(string AddminOtherRole)
        {
            ViewBag.AddminOtherRole = AddminOtherRole;
            return View();
        }

        [HttpGet]
        public ActionResult EnroleForMicroCredential()
        {
            var dictionary = _repositoryEndPointService.GetCurrentUserAndCourses(User.Identity.Name.ToLower());
            if (dictionary.Keys.Any())
            {
                ViewBag.MicroCredentials = dictionary.Values.First();
                var model = new EnroleForMicroCredentialViewModel { CandidateId = dictionary.Keys.First().CandidateId };

                return View(model);
            }
            var selectList = new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            ViewBag.MicroCredentials = selectList;
            return View(new EnroleForMicroCredentialViewModel { CandidateId = -1, MicroCredentialId = -1 });
        }
        [HttpPost]
        public ActionResult EnroleForMicroCredential(EnroleForMicroCredentialViewModel model)
        {
            if (ModelState.IsValid)
            {
                _repositoryEndPointService.EnroleUserToMicroCredentialCourse(model.CandidateId, model.MicroCredentialId);
                return View("Success");
            }

            var selectList = new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            ViewBag.MicroCredentials = selectList;
            return View(model);
        }
        // GET: Candidate/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetCandidateDigitalBadgesByEmail(string candidateEmail)
        {
            var candidateBadges = _repositoryEndPointService.GetCandidateDigitalBadgesByEmail(candidateEmail);

            ViewBag.CertificateImageUrl = candidateBadges.Select(p => "/images/Badges/" + candidateEmail + "_" + p.MicroCredentialBadges + ".jpg").ToArray();

            return View("CandidateBadgesView", candidateBadges);
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult ViewCandidateDigitalBadgesByEmail()
        {
            var candidateBadges = _repositoryEndPointService.GetCandidateDigitalBadgesByEmail(User.Identity.Name);

            ViewBag.CertificateImageUrl = candidateBadges.Select(p => "/images/Badges/" + User.Identity.Name+"_"+p.MicroCredentialBadges+".jpg").ToArray();
            return PartialView("_CandidateMicroCredentialBadges", candidateBadges);
        }
        [HttpGet]
        public ActionResult ListJobs()
        {
            Job[] jobs = _repositoryEndPointService.GetAllJobs();
            ViewBag.EmployerIdList = GetEmployerIds();

            var mapper = AutoMapperConfig.Configure();
            var jobsViewModel = mapper.Map<Job[], JobViewModel[]>(jobs);
            return View("CandidateJobList", jobsViewModel);
        }
        [HttpGet]
        public ActionResult ApplyForJob(int id = -1)
        {
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            var candidate = _repositoryEndPointService.GetCandidateByEmailAddress(User.Identity.Name.ToLower());
            if (candidate != null)
            {
                ViewBag.CandidateId = candidate.CandidateId;
            }
            else
            {
                ViewBag.CandidateId = -1;
                return View("Failed");
            }
            var job = _repositoryEndPointService.GetJobsById(id);
            if (job == null) return View("Failed");
            var mapper = AutoMapperConfig.Configure();
            var jobViewModel = mapper.Map<Job, JobViewModel>(job);
            return View(jobViewModel);
        }
        [HttpPost]
        public ActionResult ApplyForJob(JobViewModel jobViewModel)
        {
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            var candidate = _repositoryEndPointService.GetCandidateByEmailAddress(User.Identity.Name.ToLower());
            var candidateJobApplicationViewModel = new CandidateJobApplicationViewModel
            {
                CandidateId = candidate.CandidateId,
                EmployerId = jobViewModel.EmployerId,
                JobId = jobViewModel.JobId
            };
            ViewBag.CandidateId = candidate.CandidateId;

            if (ModelState.IsValid)
            {

                var mapper = AutoMapperConfig.Configure();
                var candidateApplication = mapper.Map<CandidateJobApplicationViewModel, CandidateJobApplication>(candidateJobApplicationViewModel);
                _repositoryEndPointService.AddNewCandidateJobApplication(candidateApplication);
                //Send Application to Employer Via EmailServices:

                return View("Success");
            }
            return View(jobViewModel);
        }

        [HttpGet]
        public ActionResult ViewUserMicroCredentialBadges()
        {
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.MicroCredentialBadgeIdList = GetCandidateMicroCredentialBadgesIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            var user = _repositoryEndPointService.GetCandidateByEmail(User.Identity.Name);
            if (user != null)
            {
                UserMicroCredentialBadges[] microCredentialBadges = _repositoryEndPointService.GetUserMicroCredentialBadgesById(user.CandidateId);
                ViewBag.CertificateImageUrl = microCredentialBadges.Select(p => "/images/Badges/" + User.Identity.Name + "_" + p.MicroCredentialBadges + ".jpg").ToArray();
                var listOfUserBadges = new List<MicroCredentialBadgeWithMicroCredentialNameViewModel>();

                foreach (var mb in microCredentialBadges)
                {
                    listOfUserBadges.Add(new MicroCredentialBadgeWithMicroCredentialNameViewModel { MicroCredentialBadges = mb.MicroCredentialBadges, Username = mb.Username, MicroCredentialBadgeId = mb.MicroCredentialBadgeId, MicroCredentialName = _unitOfWork.MicroCredentialRepository.GetById(mb.MicroCredentialId).MicroCredentialName });
                }
                return View(listOfUserBadges.ToArray());
            }
            return View("Failed");
        }
        // GET: Candidate/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Candidate/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Candidate/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Candidate/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
