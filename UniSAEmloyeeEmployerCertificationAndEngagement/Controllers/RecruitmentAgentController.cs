using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RSA316Infinito.BigRsaCrypto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
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
    [Authorize(Roles = "RecruitmentAgency,Administrator")]
    public class RecruitmentAgentController : Controller
    {
        UnitOfWork _unitOfWork;
        private RoleManager<IdentityRole> _roleManager;
        private RepositoryEndPointServices _repositoryEndPointService;
        public RecruitmentAgentController(UnitOfWork unitOfWork)
        {
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            _unitOfWork = unitOfWork;
            _unitOfWork.UniSADbContext = new UniSA.DataAccess.UniSADbContext();
            var moocMicroCredentialProvider = new MoocMicroCredentialProvider(new StratisEndPointAdhocService(), new EncryptDecrypt());
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["StratisBlockChainBaseUrl"]);
            moocMicroCredentialProvider.StratisApiFullfilRequestComponent = new UniSA.Services.StratisBlockChainServices.StratisApi.StratisApiFullfilRequestComponent(client);
            _repositoryEndPointService = new RepositoryEndPointServices(_unitOfWork, moocMicroCredentialProvider);
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
            return _unitOfWork.CandidateMicroCredentialCourseRepository.GetAll().Select(a => new SelectListItem { Text = a.Candidate.EmailAddress, Value = a.CandidateMicroCredentialCourseId.ToString() }).ToList();
        }
        private List<SelectListItem> GetMoocProviderIds()
        {
            return _unitOfWork.MoocProviderRepository.GetAll().Select(a => new SelectListItem { Text = a.MoocProviderName, Value = a.MoocProviderId.ToString() }).ToList();
        }
        private List<SelectListItem> GetRecrutimentAgentIds()
        {
            return _unitOfWork.RecruitmentAgencyRepository.GetAll().Select(a => new SelectListItem { Text = a.RecruitmentAgencyName, Value = a.RecruitmentAgencyId.ToString() }).ToList();
        }
        [HttpGet]
        public ActionResult Index(string AddminOtherRole)
        {
            ViewBag.AddminOtherRole = AddminOtherRole;
            return View();
        }
        [HttpGet]
        public ActionResult CreateReadUpdateUserRecruitmentAgent()
        {
            ViewBag.RecruitmentAgentIdList = GetRecrutimentAgentIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateUserRecruitmentAgent");
        }

        [HttpGet]
        public ActionResult SelectUserRecruitmentAgent()
        {
            ViewBag.RecruitmentAgentIdList = GetRecrutimentAgentIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateUserRecruitmentAgent");
        }
        [HttpPost]
        public ActionResult SelectUserRecruitmentAgent(SelectDeleteRecruitmentAgentViewModel recruitmentAgentViewModel)
        {
            ViewBag.RecruitmentAgentIdList = GetRecrutimentAgentIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.AddressIdList = GetAddressIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                ModelState.Clear();
                RecruitmentAgency recruitmentAgencies = _repositoryEndPointService.GetRecruitmentAgencyById(recruitmentAgentViewModel.RecruitmentAgencyId);
                recruitmentAgentViewModel = mapper.Map<RecruitmentAgency, SelectDeleteRecruitmentAgentViewModel>(recruitmentAgencies);
                return View("SelectDeleteUserRecruitmentAgent ", recruitmentAgentViewModel);
            }
            return View("SelectDeleteUserRecruitmentAgent ", recruitmentAgentViewModel);
        }

        [HttpGet]
        public ActionResult DeleteUserRecruitmentAgent()
        {
            ViewBag.RecruitmentAgentIdList = GetRecrutimentAgentIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateUserRecruitmentAgent");
        }
        [HttpPost]
        public ActionResult DeleteUserRecruitmentAgent(SelectDeleteRecruitmentAgentViewModel recruitmentAgentViewModel)
        {
            ViewBag.RecruitmentAgentIdList = GetRecrutimentAgentIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.AddressIdList = GetAddressIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                var recruitmentAgency = mapper.Map<SelectDeleteRecruitmentAgentViewModel, RecruitmentAgency>(recruitmentAgentViewModel);
                _repositoryEndPointService.DeletetRecruitmentAgency(recruitmentAgency);
                return View("Success");
            }
            return View("SelectDeleteUserRecruitmentAgent", recruitmentAgentViewModel);
        }
        [HttpGet]
        public ActionResult UpdateUserRecruitmentAgent()
        {
            ViewBag.RecruitmentAgentIdList = GetRecrutimentAgentIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateUserRecruitmentAgent");
        }
        [HttpPost]
        public ActionResult UpdateUserRecruitmentAgent(RecruitmentAgentViewModel recruitmentAgentViewModel)
        {
            ViewBag.RecruitmentAgentIdList = GetRecrutimentAgentIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.AddressIdList = GetAddressIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                var recruitmentAgency = mapper.Map<RecruitmentAgentViewModel, RecruitmentAgency>(recruitmentAgentViewModel);
                _repositoryEndPointService.UpdatetRecruitmentAgency(recruitmentAgency);
                return View("Success");
            }
            return View("CreateReadUpdateUserRecruitmentAgent", recruitmentAgentViewModel);
        }
        [HttpGet]
        public ActionResult InsertUserRecruitmentAgent()
        {
            ViewBag.RecruitmentAgentIdList = GetRecrutimentAgentIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateUserRecruitmentAgent");
        }
        [HttpPost]
        public ActionResult InsertUserRecruitmentAgent(RecruitmentAgentViewModel recruitmentAgentViewModel)
        {
            ViewBag.RecruitmentAgentIdList = GetRecrutimentAgentIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.AddressIdList = GetAddressIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                var recruitmentAgency = mapper.Map<RecruitmentAgentViewModel, RecruitmentAgency>(recruitmentAgentViewModel);
                _repositoryEndPointService.InsertRecruitmentAgency(recruitmentAgency);
                return View("Success");
            }
            return View("CreateReadUpdateUserRecruitmentAgent", recruitmentAgentViewModel);
        }
        // GET: RecruitmentAgent/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RecruitmentAgent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RecruitmentAgent/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: RecruitmentAgent/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RecruitmentAgent/Edit/5
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

        // GET: RecruitmentAgent/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RecruitmentAgent/Delete/5
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
