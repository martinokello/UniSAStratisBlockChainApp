using Autofac.Features.Variance;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RSA316Infinito.BigRsaCrypto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
    [Authorize(Roles = "MoocProvider,Administrator")]
    public class MoocProviderController : Controller
    {
        UnitOfWork _unitOfWork;
        RepositoryEndPointServices _repositoryEndPointService;
        private RoleManager<IdentityRole> _roleManager;

        public MoocProviderController(UnitOfWork unitOfWork)
        {
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            _unitOfWork = unitOfWork;
            _unitOfWork.UniSADbContext = new UniSA.DataAccess.UniSADbContext();
            var moocMicroCredentialProvider = new MoocMicroCredentialProvider(new StratisEndPointAdhocService(), new EncryptDecrypt());

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["StratisBlockChainBaseUrl"]);
            moocMicroCredentialProvider.StratisApiFullfilRequestComponent = new UniSA.Services.StratisBlockChainServices.StratisApi.StratisApiFullfilRequestComponent(client);
            _repositoryEndPointService = new RepositoryEndPointServices(_unitOfWork, moocMicroCredentialProvider);
            moocMicroCredentialProvider.RepositoryEndPointService = _repositoryEndPointService;
        }
        [HttpGet]
        public ActionResult Index(string AddminOtherRole)
        {
            ViewBag.AddminOtherRole = AddminOtherRole;
            return View();
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
            if (User.IsInRole("Adminstrator"))
            {
                return _unitOfWork.MicroCredentialRepository.GetAll().Select(a => new SelectListItem { Text = a.MicroCredentialName, Value = a.MicroCredentialId.ToString() }).ToList();
            }
            var moocProvider = _repositoryEndPointService.GetMoocProviderByEmail(User.Identity.Name);
            if(moocProvider == null) return _unitOfWork.MicroCredentialRepository.GetAll().Select(a => new SelectListItem { Text = a.MicroCredentialName, Value = a.MicroCredentialId.ToString() }).ToList();

            return _unitOfWork.MicroCredentialRepository.GetAll().Where(m => m.MoocProviderId == moocProvider.MoocProviderId).Select(a => new SelectListItem { Text = a.MicroCredentialName, Value = a.MicroCredentialId.ToString() }).ToList<SelectListItem>();

        }
        private List<SelectListItem> GetGovernmentIds()
        {
            return _unitOfWork.GorvernmentRepository.GetAll().Select(a => new SelectListItem { Text = a.GovernmentDepartmentName, Value = a.DepartmentAddressId.ToString() }).ToList();
        }
        private List<SelectListItem> GetCandidateMicroCredentialCourseIds()
        {
            if (User.IsInRole("Adminstrator"))
            {
                return _unitOfWork.CandidateMicroCredentialCourseRepository.GetAll().Select(a => new SelectListItem { Text = "[" + a.MicroCredential.MicroCredentialName + "@" + a.Candidate.EmailAddress + "]", Value = a.CandidateMicroCredentialCourseId.ToString() }).ToList();
            }

            var moocProvider = _repositoryEndPointService.GetMoocProviderByEmail(User.Identity.Name);
            var items = from mc in _unitOfWork.MicroCredentialRepository.GetAll().Where(m => m.MoocProviderId == moocProvider.MoocProviderId)
                        join crs in _unitOfWork.CandidateMicroCredentialCourseRepository.GetAll()
                        on mc.MicroCredentialId equals crs.MicroCredentialId
                        select new SelectListItem { Text = mc.MicroCredentialName, Value = mc.MicroCredentialId.ToString() };
            return items.ToList<SelectListItem>();
        }
        private List<SelectListItem> GetMoocProviderIds()
        {
            return _unitOfWork.MoocProviderRepository.GetAll().Select(a => new SelectListItem { Text = a.MoocProviderName, Value = a.MoocProviderId.ToString() }).ToList();
        }
        private List<SelectListItem> GetMicroCredentialBadgeIds()
        {
            return _unitOfWork.CandidatetMicroCredentialBadgesRepository.GetAll().Select(a => new SelectListItem { Text = a.MicroCredentialBadges, Value = a.MicroCredentialBadgeId.ToString() }).ToList();
        }
        private List<SelectListItem> GetRecrutimentAgentIds()
        {
            return _unitOfWork.RecruitmentAgencyRepository.GetAll().Select(a => new SelectListItem { Text = a.RecruitmentAgencyName, Value = a.RecruitmentAgencyId.ToString() }).ToList();
        }
        [HttpGet]
        public ActionResult SelectMicroCredential()
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.AccreditedBodyIdList = GetAccreditationBodyIds();
            ViewBag.MoocProviderIdList = GetMoocProviderIds();
            var microCredentialViewModel = new SelectDeleteMicroCredentialViewModel
            {
                CertificateFee = 0,
                DurationEnd = DateTime.Now.Date,
                DurationStart = DateTime.Now.Date,
                IsAccredited = false,
                IsEndorsed = false,
                Fee = 0,
                NumberOfCredits = 0
            };

            return View("SelectMicroCredential", microCredentialViewModel);
        }
        [HttpPost]
        public ActionResult SelectMicroCredential(SelectDeleteMicroCredentialViewModel microCredentialViewModel)
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.AccreditedBodyIdList = GetAccreditationBodyIds();
            ViewBag.MoocProviderIdList = GetMoocProviderIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                ModelState.Clear();
                MicroCredential microCredential = _repositoryEndPointService.GetMicroCredentialById(microCredentialViewModel.MicroCredentialId);
                microCredentialViewModel = mapper.Map<MicroCredential, SelectDeleteMicroCredentialViewModel>(microCredential);
                return View("SelectMicroCredential", microCredentialViewModel);
            }
            return View("SelectMicroCredential", microCredentialViewModel);
        }
        [HttpGet]
        public ActionResult UpdateMicroCredential()
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.AccreditedBodyIdList = GetAccreditationBodyIds();
            ViewBag.MoocProviderIdList = GetMoocProviderIds();
            var microCredentialViewModel = new MicroCredentialViewModel
            {
                CertificateFee = 0,
                DurationEnd = DateTime.Now.Date,
                DurationStart = DateTime.Now.Date,
                IsAccredited = false,
                IsEndorsed = false,
                Fee = 0,
                NumberOfCredits = 0
            };

            return View("CreateMicroCredential", microCredentialViewModel);
        }
        [HttpPost]
        public ActionResult UpdateMicroCredential(MicroCredentialViewModel microCredentialViewModel)
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.AccreditedBodyIdList = GetAccreditationBodyIds();
            ViewBag.MoocProviderIdList = GetMoocProviderIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                var microCredential = mapper.Map<MicroCredentialViewModel, MicroCredential>(microCredentialViewModel);
                _repositoryEndPointService.UpdateMicroCredential(microCredential);
                return View("Success");
            }
            return View("CreateMicroCredential", microCredentialViewModel);
        }
        [HttpGet]
        public ActionResult InsertMicroCredential()
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.AccreditedBodyIdList = GetAccreditationBodyIds();
            ViewBag.MoocProviderIdList = GetMoocProviderIds();
            var microCredentialViewModel = new MicroCredentialViewModel
            {
                CertificateFee = 0,
                DurationEnd = DateTime.Now.Date,
                DurationStart = DateTime.Now.Date,
                IsAccredited = false,
                IsEndorsed = false,
                Fee = 0,
                NumberOfCredits = 0
            };

            return View("CreateMicroCredential", microCredentialViewModel);
        }
        [HttpPost]
        public ActionResult InsertMicroCredential(MicroCredentialViewModel microCredentialViewModel)
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.AccreditedBodyIdList = GetAccreditationBodyIds();
            ViewBag.MoocProviderIdList = GetMoocProviderIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                var microCredential = mapper.Map<MicroCredentialViewModel, MicroCredential>(microCredentialViewModel);
                _repositoryEndPointService.InsertMicroCredential(microCredential);
                return View("Success");
            }
            return View("CreateMicroCredential", microCredentialViewModel);
        }


        [HttpGet]
        public ActionResult CreateUserMicroCredentialBadge()
        {
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.MicroCredentialBadgeIdList = GetCandidateMicroCredentialCourseIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            return View();
        }
        [HttpPost]
        public ActionResult CreateUserMicroCredentialBadge(MicroCredentialBadgeViewModel microCredentialBadgeViewModel)
        {
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.MicroCredentialBadgeIdList = GetCandidateMicroCredentialCourseIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                UserMicroCredentialBadges badge = mapper.Map<MicroCredentialBadgeViewModel, UserMicroCredentialBadges>(microCredentialBadgeViewModel);
                _unitOfWork.CandidatetMicroCredentialBadgesRepository.Add(badge);
                _unitOfWork.SaveChanges();
                return View("Success");

            }
            return View(microCredentialBadgeViewModel);
        }
        // POST: MoocProvider/Create
        [HttpGet]
        public ActionResult MoocProviderCertifyUser()
        {
            ViewBag.MicroCredentialBadgesIdList = GetMicroCredentialBadgeIds();
            ViewBag.CandidateMicroCredentialCourseIdList = GetCandidateMicroCredentialCourseIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.CandidateIdList = GetCandidateIds();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> MoocProviderCertifyUser(CandidateMicroCredentialCourseViewModel candidateMicroCredentialCourseViewModel)
        {
            ViewBag.CandidateMicroCredentialCourseIdList = GetCandidateMicroCredentialCourseIds();
            ViewBag.MicroCredentialBadgesIdList = GetMicroCredentialBadgeIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.CandidateIdList = GetCandidateIds();

            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                var candidateMicroCredentialCourse = mapper.Map<CandidateMicroCredentialCourseViewModel, CandidateMicroCredentialCourse>(candidateMicroCredentialCourseViewModel);
                bool userCertified = await _repositoryEndPointService.CertifyCandidateUser(candidateMicroCredentialCourse);
                if (userCertified) return View("Success");
                return View("Failed");
            }
            return View();
        }
        [HttpGet]
        public ActionResult EnroleUserToCourse()
        {
            ViewBag.CandidateMicroCredentialCourseIdList = GetCandidateMicroCredentialCourseIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.CandidateIdList = GetCandidateIds();
            return View();
        }
        [HttpPost]
        public ActionResult EnroleUserToCourse(CandidateMicroCredentialCourseViewModel candidateMicroCredentialCourseViewModel)
        {
            ViewBag.CandidateMicroCredentialCourseIdList = GetCandidateMicroCredentialCourseIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.CandidateIdList = GetCandidateIds();

            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                var candidateMicroCredentialCourse = mapper.Map<CandidateMicroCredentialCourseViewModel, CandidateMicroCredentialCourse>(candidateMicroCredentialCourseViewModel);
                _repositoryEndPointService.EnroleUserToMicroCredentialCourse(candidateMicroCredentialCourse.CandidateId, candidateMicroCredentialCourse.MicroCredentialId);
                return View("Success");
            }
            return View();
        }
        [HttpGet]
        public ActionResult ValidateCandidateMicroCredential()
        {
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
        public ActionResult ViewUserMicroCredentialBadges()
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            var userMicCredBadges = (from bg in _unitOfWork.CandidatetMicroCredentialBadgesRepository.GetAll().ToArray()
                                    join m in GetMicroCredentialIds()
                                    on bg.MicroCredentialId.ToString() equals m.Value
                                    select bg).ToArray();
            var microCredentialBadgesViewModel = AutoMapperConfig.Configure().Map(userMicCredBadges, typeof(UserMicroCredentialBadges[]), typeof(UserMicroCredentialBadgesViewModel[])) as UserMicroCredentialBadgesViewModel[];

            return View(microCredentialBadgesViewModel);
        }

        // GET: MoocProvider/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MoocProvider/Edit/5
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

        // GET: MoocProvider/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MoocProvider/Delete/5
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
