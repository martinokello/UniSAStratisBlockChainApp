using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RSA316Infinito.BigRsaCrypto;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    [Authorize(Roles = "Administrator")]
    public class AdministrationController : Controller
    {
        UnitOfWork _unitOfWork;
        private RepositoryEndPointServices _repositoryEndPointService;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationUserManager _userManager;
        public AdministrationController(UnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager, ApplicationUserManager userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _unitOfWork.UniSADbContext = new UniSA.DataAccess.UniSADbContext();
            var moocMicroCredentialProvider = new MoocMicroCredentialProvider(new StratisEndPointAdhocService(), new EncryptDecrypt());
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["StratisBlockChainBaseUrl"]);
            moocMicroCredentialProvider.StratisApiFullfilRequestComponent = new UniSA.Services.StratisBlockChainServices.StratisApi.StratisApiFullfilRequestComponent(client);
            _repositoryEndPointService = new RepositoryEndPointServices(_unitOfWork, moocMicroCredentialProvider);
            moocMicroCredentialProvider.RepositoryEndPointService = _repositoryEndPointService;

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

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AssignUserToRole()
        {
            ViewBag.RoleList = GetRoles();
            return View();
        }
        [HttpPost]
        public ActionResult AssignUserToRole(UserRoleViewModel userRoleViewModel)
        {
            ViewBag.RoleList = GetRoles();
            if (ModelState.IsValid)
            {
                var apUser = _userManager.FindByEmail(userRoleViewModel.Username);
                if (apUser != null && _roleManager.RoleExists(userRoleViewModel.RoleName))
                {
                    var idUserRole = new IdentityUserRole();
                    idUserRole.UserId = apUser.Id;
                    var role = _roleManager.Roles.FirstOrDefault(r => r.Name.ToLower().Equals(userRoleViewModel.RoleName.ToLower()));

                    if (!string.IsNullOrEmpty(userRoleViewModel.Insert))
                    {
                        //var adminRole = _roleManager.FindByName("Administrator");
                        if (apUser.Roles.FirstOrDefault(ur => ur.RoleId == role.Id) == null)
                        {
                            _userManager.AddToRole(apUser.Id, userRoleViewModel.RoleName);
                        }
                        return View("Success");
                    }
                    else if (!string.IsNullOrEmpty(userRoleViewModel.Delete))
                    {
                        _userManager.RemoveFromRole(apUser.Id, userRoleViewModel.RoleName);
                        return View("Success");
                    }
                }
                return View("Failed");
            }
            return View(userRoleViewModel);
        }
        [HttpGet]
        public ActionResult CreateReadUpdateAccreditationBody()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.AccreditationBodyIdList = GetAccreditationBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            return View("CreateReadUpdateAccreditationBody");
        }

        [HttpGet]
        public ActionResult SelectAccreditationBody()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.AccreditationBodyIdList = GetAccreditationBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            return View("CreateReadUpdateAccreditationBody");
        }
        [HttpPost]
        public ActionResult SelectAccreditationBody(SelectDeleteAccreditationBodyViewModel accreditationBodyViewModel)
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.AccreditationBodyIdList = GetAccreditationBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();

            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                ModelState.Clear();
                var accreditationBody = _repositoryEndPointService.GetAccreditationBodyById(accreditationBodyViewModel.AccreditationBodyId);
                accreditationBodyViewModel = mapper.Map<AccreditationBody, SelectDeleteAccreditationBodyViewModel>(accreditationBody);
                return View("SelectDeleteAccreditationBody", accreditationBodyViewModel);
            }
            return View("SelectDeleteAccreditationBody", accreditationBodyViewModel);
        }
        [HttpGet]
        public ActionResult DeleteAccreditationBody()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.AccreditationBodyIdList = GetAccreditationBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            return View("CreateReadUpdateAccreditationBody");
        }
        [HttpPost]
        public ActionResult DeleteAccreditationBody(SelectDeleteAccreditationBodyViewModel accreditationBodyViewModel)
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.AccreditationBodyIdList = GetAccreditationBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();

            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                var accreditationBody = mapper.Map<SelectDeleteAccreditationBodyViewModel, AccreditationBody>(accreditationBodyViewModel);
                _repositoryEndPointService.DeleteAccreditationBody(accreditationBody);
                return View("Success");
            }
            return View("SelectDeleteAccreditationBody", accreditationBodyViewModel);
        }

        [HttpGet]
        public ActionResult InsertAccreditationBody()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.AccreditationBodyIdList = GetAccreditationBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            return View("CreateReadUpdateAccreditationBody");
        }
        [HttpPost]
        public ActionResult InsertAccreditationBody(AccreditationBodyViewModel accreditationBodyViewModel)
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.AccreditationBodyIdList = GetAccreditationBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                var accreditationBody = mapper.Map<AccreditationBodyViewModel, AccreditationBody>(accreditationBodyViewModel);
                _repositoryEndPointService.InsertAccreditationBody(accreditationBody);
                return View("Success");

            }
            return View("CreateReadUpdateAccreditationBody", accreditationBodyViewModel);
        }

        [HttpGet]
        public ActionResult UpdateAccreditationBody()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.AccreditationBodyIdList = GetAccreditationBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            return View("CreateReadUpdateAccreditationBody");
        }
        [HttpPost]
        public ActionResult UpdateAccreditationBody(AccreditationBodyViewModel accreditationBodyViewModel)
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.AccreditationBodyIdList = GetAccreditationBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                var accreditationBody = mapper.Map<AccreditationBodyViewModel, AccreditationBody>(accreditationBodyViewModel);
                _repositoryEndPointService.UpdateAccreditationBody(accreditationBody);
                return View("Success");
            }
            return View("CreateReadUpdateAccreditationBody", accreditationBodyViewModel);
        }
        [HttpGet]
        public ActionResult CreateReadUpdateEndorsementBody()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            return View("CreateReadUpdateEndorsementBody");
        }

        [HttpGet]
        public ActionResult SelectEndorsementBody()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            return View("CreateReadUpdateEndorsementBody");
        }
        [HttpPost]
        public ActionResult SelectEndorsementBody(SelectDeleteEndorsementBodyViewModel endorsementBodyViewModel)
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();

            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                ModelState.Clear();
                var endorsementBody = _repositoryEndPointService.GetEndorsementBodyById(endorsementBodyViewModel.EndorsementBodyId);
                endorsementBodyViewModel = mapper.Map<EndorsementBody, SelectDeleteEndorsementBodyViewModel>(endorsementBody);
                return View("SelectDeleteEndorsementBody", endorsementBodyViewModel);

            }
            return View("SelectDeleteEndorsementBody", endorsementBodyViewModel);
        }


        [HttpGet]
        public ActionResult DeleteEndorsementBody()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            return View("CreateReadUpdateEndorsementBody");
        }
        [HttpPost]
        public ActionResult DeleteEndorsementBody(SelectDeleteEndorsementBodyViewModel endorsementBodyViewModel)
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();

            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                var endorsementBody = mapper.Map<SelectDeleteEndorsementBodyViewModel, EndorsementBody>(endorsementBodyViewModel);
                _repositoryEndPointService.DeleteEndorsementBody(endorsementBody);
                return View("Success");

            }
            return View("SelectDeleteEndorsementBody", endorsementBodyViewModel);
        }

        [HttpGet]
        public ActionResult UpdateEndorsementBody()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            return View("CreateReadUpdateEndorsementBody");
        }
        [HttpPost]
        public ActionResult UpdateEndorsementBody(EndorsementBodyViewModel endorsementBodyViewModel)
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                var endorsementBody = mapper.Map<EndorsementBodyViewModel, EndorsementBody>(endorsementBodyViewModel);
                _repositoryEndPointService.UpdateEndorsementBody(endorsementBody);
                return View("Success");
            }
            return View("CreateReadUpdateEndorsementBody", endorsementBodyViewModel);
        }
        [HttpGet]
        public ActionResult InsertEndorsementBody()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            return View("CreateReadUpdateEndorsementBody");
        }
        [HttpPost]
        public ActionResult InsertEndorsementBody(EndorsementBodyViewModel endorsementBodyViewModel)
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                var endorsementBody = mapper.Map<EndorsementBodyViewModel, EndorsementBody>(endorsementBodyViewModel);
                _repositoryEndPointService.InsertEndorsementBody(endorsementBody);
                return View("Success");
            }
            return View("CreateReadUpdateEndorsementBody", endorsementBodyViewModel);
        }
        [HttpGet]
        public ActionResult CreateReadUpdateAddress()
        {
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateAddress");
        }
        [HttpGet]
        public ActionResult UpdateAddress()
        {
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateAddress");
        }
        [HttpPost]
        public ActionResult UpdateAddress(AddressViewModel addressViewModel)
        {

            ViewBag.AddressIdList = GetAddressIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                var address = mapper.Map<AddressViewModel, Address>(addressViewModel);
                _repositoryEndPointService.UpdateAddress(address);
                return View("Success");

            }
            return View("CreateReadUpdateAddress", addressViewModel);
        }
        [HttpGet]
        public ActionResult InsertAddress()
        {
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateAddress");
        }
        [HttpPost]
        public ActionResult InsertAddress(AddressViewModel addressViewModel)
        {

            ViewBag.AddressIdList = GetAddressIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                var address = mapper.Map<AddressViewModel, Address>(addressViewModel);
                _repositoryEndPointService.InsertAddress(address);
                return View("Success");
            }
            return View("CreateReadUpdateAddress", addressViewModel);
        }
        [HttpGet]
        public ActionResult SelectAddress()
        {
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateAddress");
        }
        [HttpPost]
        public ActionResult SelectAddress(SelectDeleteAddressViewModel addressViewModel)
        {

            ViewBag.AddressIdList = GetAddressIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                ModelState.Clear();
                var address = _repositoryEndPointService.GetAddressById(addressViewModel.AddressId);
                addressViewModel = mapper.Map<Address, SelectDeleteAddressViewModel>(address);
                return View("SelectDeleteAddress", addressViewModel);

            }
            return View("SelectDeleteAddress", addressViewModel);
        }
        [HttpGet]
        public ActionResult DeleteAddress()
        {
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateAddress");
        }
        [HttpPost]
        public ActionResult DeleteAddress(SelectDeleteAddressViewModel addressViewModel)
        {

            ViewBag.AddressIdList = GetAddressIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                var address = mapper.Map<SelectDeleteAddressViewModel, Address>(addressViewModel);
                _repositoryEndPointService.DeleteAddress(address);
                return View("Success");
            }
            return View("SelectDeleteAddress", addressViewModel);
        }

        [HttpGet]
        public ActionResult CreateReadUpdateCandidate()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();

            return View("CreateReadUpdateCandidate");
        }

        [HttpGet]
        public ActionResult SelectCandidate()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();

            return View("CreateReadUpdateCandidate");
        }
        [HttpPost]
        public ActionResult SelectCandidate(SelectDeleteCandidateViewModel candidateViewModel)
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                ModelState.Clear();
                var candidate = _repositoryEndPointService.GetCandidateById(candidateViewModel.CandidateId);
                candidateViewModel = mapper.Map<Candidate, SelectDeleteCandidateViewModel>(candidate);
                return View("SelectDeleteCandidate", candidateViewModel);
            }
            return View("SelectDeleteCandidate", candidateViewModel);
        }

        [HttpGet]
        public ActionResult DeleteCandidate()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();

            return View("CreateReadUpdateCandidate");
        }
        [HttpPost]
        public ActionResult DeleteCandidate(SelectDeleteCandidateViewModel candidateViewModel)
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                var candidate = mapper.Map<SelectDeleteCandidateViewModel, Candidate>(candidateViewModel);
                _repositoryEndPointService.DeleteCandidate(candidate);
                return View("Success");
            }
            return View("SelectDeleteCandidate", candidateViewModel);
        }
        [HttpGet]
        public ActionResult UpdateCandidate()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();

            return View("CreateReadUpdateCandidate");
        }
        [HttpPost]
        public ActionResult UpdateCandidate(CandidateViewModel candidateViewModel)
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                var mapper = AutoMapperConfig.Configure();
                var candidate = mapper.Map<CandidateViewModel, Candidate>(candidateViewModel);
                _repositoryEndPointService.UpdateCandidate(candidate);
                return View("Success");
            }
            return View("CreateReadUpdateCandidate", candidateViewModel);
        }
        [HttpGet]
        public ActionResult InsertCandidate()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();

            return View("CreateReadUpdateCandidate");
        }
        [HttpPost]
        public ActionResult InsertCandidate(CandidateViewModel candidateViewModel)
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                var candidate = mapper.Map<CandidateViewModel, Candidate>(candidateViewModel);
                _repositoryEndPointService.InsertCandidate(candidate);
                return View("Success");
            }
            return View("CreateReadUpdateCandidate", candidateViewModel);
        }
        [HttpGet]
        public ActionResult CreateReadUpdateEmployer()
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.JobsIdList = GetJobIds();

            return View("CreateReadUpdateEmployer");
        }

        [HttpGet]
        public ActionResult UpdateEmployer()
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.JobsIdList = GetJobIds();

            return View("CreateReadUpdateEmployer");
        }
        [HttpPost]
        public ActionResult UpdateEmployer(SelectDeleteEmployerViewModel employerViewModel)
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.JobsIdList = GetJobIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                var employer = mapper.Map<SelectDeleteEmployerViewModel, Employer>(employerViewModel);
                _repositoryEndPointService.UpdateEmployer(employer);

                return View("Success");
            }
            return View("CreateReadUpdateEmployer", employerViewModel);
        }
        [HttpGet]
        public ActionResult InsertEmployer()
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.JobsIdList = GetJobIds();

            return View("CreateReadUpdateEmployer");
        }
        [HttpPost]
        public ActionResult InsertEmployer(SelectDeleteEmployerViewModel employerViewModel)
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.JobsIdList = GetJobIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                var employer = mapper.Map<SelectDeleteEmployerViewModel, Employer>(employerViewModel);
                _repositoryEndPointService.InsertEmployer(employer);
                return View("Success");
            }
            return View("CreateReadUpdateEmployer", employerViewModel);
        }
        [HttpGet]
        public ActionResult SelectEmployer()
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.JobsIdList = GetJobIds();

            return View("CreateReadUpdateEmployer");
        }
        [HttpPost]
        public ActionResult SelectEmployer(SelectDeleteEmployerViewModel employerViewModel)
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.JobsIdList = GetJobIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                ModelState.Clear();
                Employer employer = _repositoryEndPointService.GetEmployerById(employerViewModel.EmployerId);
                employerViewModel = mapper.Map<Employer, SelectDeleteEmployerViewModel>(employer);
                return View("SelectDeleteEmployer", employerViewModel);
            }
            return View("SelectDeleteEmployer", employerViewModel);
        }
        [HttpGet]
        public ActionResult DeleteEmployer()
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.JobsIdList = GetJobIds();

            return View("CreateReadUpdateEmployer");
        }
        [HttpPost]
        public ActionResult DeleteEmployer(EmployerViewModel employerViewModel)
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.JobsIdList = GetJobIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                var employer = mapper.Map<EmployerViewModel, Employer>(employerViewModel);
                _repositoryEndPointService.DeleteEmployer(employer);
                return View("Success");
            }
            return View("SelectDeleteEmployer", employerViewModel);
        }

        [HttpGet]
        public ActionResult CreateReadUpdateJob()
        {
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            var jobViewModel = new JobViewModel { DateCreated = DateTime.Now, DateUpdated = DateTime.Now, NumberOfPositions = 1 };
            return View("CreateReadUpdateJob", jobViewModel);
        }

        [HttpGet]
        public ActionResult SelectJob()
        {
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            var jobViewModel = new JobViewModel { DateCreated = DateTime.Now, DateUpdated = DateTime.Now, NumberOfPositions = 1 };
            return View("CreateReadUpdateJob", jobViewModel);
        }
        [HttpPost]
        public ActionResult SelectJob(SelectDeleteJobViewModel jobViewModel)
        {
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                ModelState.Clear();
                Job job = _repositoryEndPointService.GetJobById(jobViewModel.JobId);
                jobViewModel = mapper.Map<Job, SelectDeleteJobViewModel>(job);
                return View("SelectDeleteJob", jobViewModel);

            }
            return View("SelectDeleteJob", jobViewModel);
        }
        [HttpGet]
        public ActionResult DeleteJob()
        {
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            var jobViewModel = new JobViewModel { DateCreated = DateTime.Now, DateUpdated = DateTime.Now, NumberOfPositions = 1 };
            return View("CreateReadUpdateJob", jobViewModel);
        }
        [HttpPost]
        public ActionResult DeleteJob(SelectDeleteJobViewModel jobViewModel)
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.JobsIdList = GetJobIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                var job = mapper.Map<SelectDeleteJobViewModel, Job>(jobViewModel);
                _repositoryEndPointService.DeleteJob(job);
                return View("Success");
            }
            return View("SelectDeleteJob", jobViewModel);
        }
        [HttpGet]
        public ActionResult UpdateJob()
        {
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            var jobViewModel = new JobViewModel { DateCreated = DateTime.Now, DateUpdated = DateTime.Now, NumberOfPositions = 1 };
            return View("CreateReadUpdateJob", jobViewModel);
        }
        [HttpPost]
        public ActionResult UpdateJob(JobViewModel jobViewModel)
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.JobsIdList = GetJobIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                var job = mapper.Map<JobViewModel, Job>(jobViewModel);
                _repositoryEndPointService.UpdateJob(job);
                return View("Success");
            }
            return View("CreateReadUpdateJob", jobViewModel);
        }
        [HttpGet]
        public ActionResult InsertJob()
        {
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.JobsIdList = GetJobIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            var jobViewModel = new JobViewModel { DateCreated = DateTime.Now, DateUpdated = DateTime.Now, NumberOfPositions = 1 };
            return View("CreateReadUpdateJob", jobViewModel);
        }
        [HttpPost]
        public ActionResult InsertJob(JobViewModel jobViewModel)
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EmployerIdList = GetEmployerIds();
            ViewBag.JobsIdList = GetJobIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                var job = mapper.Map<JobViewModel, Job>(jobViewModel);
                _repositoryEndPointService.InsertJob(job);
                return View("Success");
            }
            return View("CreateReadUpdateJob", jobViewModel);
        }
        [HttpGet]
        public ActionResult CreateReadUpdateMicroCredential()
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

            return View("CreateReadUpdateMicroCredential", microCredentialViewModel);
        }

        [HttpGet]
        public ActionResult SelectMicroCredential()
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

            return View("CreateReadUpdateMicroCredential", microCredentialViewModel);
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
                return View("SelectDeleteMicroCredential", microCredentialViewModel);
            }
            return View("SelectDeleteMicroCredential", microCredentialViewModel);
        }

        [HttpGet]
        public ActionResult DeleteMicroCredential()
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

            return View("CreateReadUpdateMicroCredential", microCredentialViewModel);
        }
        [HttpPost]
        public ActionResult DeleteMicroCredential(SelectDeleteMicroCredentialViewModel microCredentialViewModel)
        {
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.AccreditedBodyIdList = GetAccreditationBodyIds();
            ViewBag.MoocProviderIdList = GetMoocProviderIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                var microCredential = mapper.Map<SelectDeleteMicroCredentialViewModel, MicroCredential>(microCredentialViewModel);
                _repositoryEndPointService.DeleteMicroCredential(microCredential);
                return View("Success");
            }
            return View("SelectDeleteMicroCredential", microCredentialViewModel);
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

            return View("CreateReadUpdateMicroCredential", microCredentialViewModel);
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
            return View("CreateReadUpdateMicroCredential", microCredentialViewModel);
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

            return View("CreateReadUpdateMicroCredential", microCredentialViewModel);
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
            return View("CreateReadUpdateMicroCredential", microCredentialViewModel);
        }
        [HttpGet]
        public ActionResult CreateReadUpdateMoocProvider()
        {
            ViewBag.MoocProviderIdList = GetMoocProviderIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateMoocProvider");
        }

        [HttpGet]
        public ActionResult SelectMoocProvider()
        {
            ViewBag.MoocProviderIdList = GetMoocProviderIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateMoocProvider");
        }
        [HttpPost]
        public ActionResult SelectMoocProvider(SelectDeleteMoocProviderViewModel moocProviderViewModel)
        {
            ViewBag.MoocProviderIdList = GetMoocProviderIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.AddressIdList = GetAddressIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                ModelState.Clear();
                MoocProvider moocProvidera = _repositoryEndPointService.GetMoocProviderById(moocProviderViewModel.MoocProviderId);
                moocProviderViewModel = mapper.Map<MoocProvider, SelectDeleteMoocProviderViewModel>(moocProvidera);
                return View("SelectDeleteMoocProvider", moocProviderViewModel);
            }
            return View("SelectDeleteMoocProvider", moocProviderViewModel);
        }

        [HttpGet]
        public ActionResult DeleteMoocProvider()
        {
            ViewBag.MoocProviderIdList = GetMoocProviderIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateMoocProvider");
        }
        [HttpPost]
        public ActionResult DeleteMoocProvider(SelectDeleteMoocProviderViewModel moocProviderViewModel)
        {
            ViewBag.MoocProviderIdList = GetMoocProviderIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.AddressIdList = GetAddressIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                var moocProvider = mapper.Map<SelectDeleteMoocProviderViewModel, MoocProvider>(moocProviderViewModel);
                _repositoryEndPointService.DeleteMoocProvider(moocProvider);
                return View("Success");
            }
            return View("SelectDeleteMoocProvider", moocProviderViewModel);
        }
        [HttpGet]
        public ActionResult UpdateMoocProvider()
        {
            ViewBag.MoocProviderIdList = GetMoocProviderIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateMoocProvider");
        }
        [HttpPost]
        public ActionResult UpdateMoocProvider(MoocProviderViewModel moocProviderViewModel)
        {
            ViewBag.MoocProviderIdList = GetMoocProviderIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.AddressIdList = GetAddressIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();
                var moocProvider = mapper.Map<MoocProviderViewModel, MoocProvider>(moocProviderViewModel);
                _repositoryEndPointService.UpdateMoocProvider(moocProvider);
                return View("Success");

            }
            return View("CreateReadUpdateMoocProvider", moocProviderViewModel);
        }
        [HttpGet]
        public ActionResult InsertMoocProvider()
        {
            ViewBag.MoocProviderIdList = GetMoocProviderIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateMoocProvider");
        }
        [HttpPost]
        public ActionResult InsertMoocProvider(MoocProviderViewModel moocProviderViewModel)
        {
            ViewBag.MoocProviderIdList = GetMoocProviderIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.AddressIdList = GetAddressIds();
            if (ModelState.IsValid)
            {
                var mapper = AutoMapperConfig.Configure();

                var moocProvider = mapper.Map<MoocProviderViewModel, MoocProvider>(moocProviderViewModel);
                _repositoryEndPointService.InsertMoocProvider(moocProvider);
                return View("Success");
            }
            return View("CreateReadUpdateMoocProvider", moocProviderViewModel);
        }
        [HttpGet]
        public ActionResult CreateReadUpdateUserMicroCredentialBadge()
        {
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.MicroCredentialBadgeIdList = GetCandidateMicroCredentialBadgesIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            return View("CreateReadUpdateUserMicroCredentialBadge");
        }

        [HttpGet]
        public ActionResult SelectUserMicroCredentialBadge()
        {
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.MicroCredentialBadgeIdList = GetCandidateMicroCredentialBadgesIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            return View("CreateReadUpdateUserMicroCredentialBadge");
        }
        [HttpPost]
        public ActionResult SelectUserMicroCredentialBadge(SelectDeleteMicroCredentialBadgeViewModel microCredentialBadgeViewModel)
        {
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.MicroCredentialBadgeIdList = GetCandidateMicroCredentialBadgesIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            if (ModelState.IsValid)
            {
                var user = _repositoryEndPointService.GetCandidateByEmail(User.Identity.Name);
                if (user != null)
                {
                    UserMicroCredentialBadges[] microCredentialBadges = _repositoryEndPointService.GetUserMicroCredentialBadgesById(user.CandidateId);
                    var microCredentialBadge = microCredentialBadges.FirstOrDefault(m => m.MicroCredentialBadgeId == microCredentialBadgeViewModel.MicroCredentialBadgeId);
                    return View(microCredentialBadge);
                }
            }
            return View("SelectDeleteUserMicroCredentialBadge", microCredentialBadgeViewModel);
        }
        [HttpGet]
        public ActionResult DeleteUserMicroCredentialBadge()
        {
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.MicroCredentialBadgeIdList = GetCandidateMicroCredentialBadgesIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            return View("CreateReadUpdateUserMicroCredentialBadge");
        }
        [HttpPost]
        public ActionResult DeleteUserMicroCredentialBadge(SelectDeleteMicroCredentialBadgeViewModel microCredentialBadgeViewModel)
        {
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.MicroCredentialBadgeIdList = GetCandidateMicroCredentialCourseIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            if (ModelState.IsValid)
            {
                var user = _repositoryEndPointService.GetCandidateByEmail(User.Identity.Name);
                if (user != null)
                {
                    UserMicroCredentialBadges[] microCredentialBadges = _repositoryEndPointService.GetUserMicroCredentialBadgesById(user.CandidateId);
                    var microCredentialBadge = microCredentialBadges.FirstOrDefault(m => m.MicroCredentialBadgeId == microCredentialBadgeViewModel.MicroCredentialBadgeId);
                    _unitOfWork.CandidatetMicroCredentialBadgesRepository.Delete(microCredentialBadge);
                    _unitOfWork.SaveChanges();
                    return View("Success");
                }
            }
            return View("SelectDeleteUserMicroCredentialBadge", microCredentialBadgeViewModel);
        }
        [HttpGet]
        public ActionResult UpdateUserMicroCredentialBadge()
        {
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.MicroCredentialBadgeIdList = GetCandidateMicroCredentialBadgesIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            return View("CreateReadUpdateUserMicroCredentialBadge");
        }
        [HttpPost]
        public ActionResult UpdateUserMicroCredentialBadge(MicroCredentialBadgeViewModel microCredentialBadgeViewModel)
        {
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.MicroCredentialBadgeIdList = GetCandidateMicroCredentialCourseIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            if (ModelState.IsValid)
            {
                UserMicroCredentialBadges microCredentialBadges = _repositoryEndPointService.GetUserMicroCredentialBadgesByBadgeId(microCredentialBadgeViewModel.MicroCredentialBadgeId);
                _unitOfWork.CandidatetMicroCredentialBadgesRepository.Update(microCredentialBadges);
                _unitOfWork.SaveChanges();
                return View("Success");

            }
            return View("CreateReadUpdateUserMicroCredentialBadge", microCredentialBadgeViewModel);
        }
        [HttpGet]
        public ActionResult InsertUserMicroCredentialBadge()
        {
            ViewBag.CandidateIdList = GetCandidateIds();
            ViewBag.MicroCredentialBadgeIdList = GetCandidateMicroCredentialBadgesIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            return View("CreateReadUpdateUserMicroCredentialBadge");
        }
        [HttpPost]
        public ActionResult InsertUserMicroCredentialBadge(MicroCredentialBadgeViewModel microCredentialBadgeViewModel)
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
            return View("CreateReadUpdateUserMicroCredentialBadge", microCredentialBadgeViewModel);
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
        [HttpGet]
        public ActionResult CreateReadUpdateUserGovernment()
        {
            ViewBag.GovernmentIdList = GetGovernmentIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateUserGovernment");
        }

        [HttpGet]
        public ActionResult InsertUserGovernment()
        {
            ViewBag.GovernmentIdList = GetGovernmentIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateUserGovernment");
        }
        [HttpPost]
        public ActionResult InsertUserGovernment(GovernmentViewModel governmentViewModel)
        {
            ViewBag.GovernmentIdList = GetGovernmentIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateUserGovernment", governmentViewModel);
        }
        [HttpGet]
        public ActionResult UpdateUserGovernment()
        {
            ViewBag.GovernmentIdList = GetGovernmentIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateUserGovernment");
        }
        [HttpPost]
        public ActionResult UpdateUserGovernment(GovernmentViewModel governmentViewModel)
        {
            ViewBag.GovernmentIdList = GetGovernmentIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateUserGovernment", governmentViewModel);
        }
        [HttpGet]
        public ActionResult SelectUserGovernment()
        {
            ViewBag.GovernmentIdList = GetGovernmentIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateUserGovernment");
        }
        [HttpPost]
        public ActionResult SelectUserGovernment(SelectDeleteGovernmentViewModel governmentViewModel)
        {
            ViewBag.GovernmentIdList = GetGovernmentIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("SelectDeleteUserGovernment", governmentViewModel);
        }
        [HttpGet]
        public ActionResult DeleteUserGovernment()
        {
            ViewBag.GovernmentIdList = GetGovernmentIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("CreateReadUpdateUserGovernment");
        }
        [HttpPost]
        public ActionResult DeleteUserGovernment(SelectDeleteGovernmentViewModel governmentViewModel)
        {
            ViewBag.GovernmentIdList = GetGovernmentIds();
            ViewBag.AddressIdList = GetAddressIds();
            return View("SelectDeleteUserGovernment", governmentViewModel);
        }
        //////////////////////////////////////////////////////////
        ////
        //////////////////////////////////////////////////////////
        private List<SelectListItem> GetStratisAccountIdList()
        {
            if (_unitOfWork.StratisAccountRepository.GetAll().Any())
            {
                return _unitOfWork.StratisAccountRepository.GetAll().Select(r => new SelectListItem { Text = r.EmailAddress+", "+r.AccountName, Value = r.StratisAccountId.ToString() }).ToList();
            }
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "No Stratis Accounts Yet", Value = (-1).ToString() });
            return list;
        }

        [HttpGet]
        public ActionResult SelectStratisAccount()
        {
            ViewBag.StratisAccountIdList = GetStratisAccountIdList();
            var stratisAccountModel = new SelectStratisAccountViewModel();
            return View(stratisAccountModel);
        }

        [HttpPost]
        public ActionResult SelectStratisAccount(SelectStratisAccountViewModel stratisAccountModel)
        {
            ViewBag.StratisAccountIdList = GetStratisAccountIdList();

            if (ModelState.IsValid)
            {
                ModelState.Clear();
                var stratisAccount = _unitOfWork.StratisAccountRepository.GetById(stratisAccountModel.StratisAccountId);
                var stratisAccountViewModel = AutoMapperConfig.Configure().Map(stratisAccount, typeof(StratisAccount), typeof(SelectStratisAccountViewModel)) as SelectStratisAccountViewModel;

                return View(stratisAccountViewModel);
            }
            return View(stratisAccountModel);
        }
/*
        [HttpGet]
        public ActionResult UpdateStratisAccount()
        {
            ViewBag.StratisAccountIdList = GetStratisAccountIdList();
            var stratisAccountModel = new StratisAccountViewModel();
            return View(stratisAccountModel);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateStratisAccount(StratisAccountViewModel stratisAccountModel)
        {
            ViewBag.StratisAccountIdList = GetStratisAccountIdList();

            if (ModelState.IsValid)
            {
                var stratisAcc = _unitOfWork.StratisAccountRepository.GetById(stratisAccountModel.StratisAccountId);
                if (stratisAcc == null)
                {
                    var moocMicroCredentialProvider = new MoocMicroCredentialProvider(new StratisEndPointAdhocService(), new EncryptDecrypt());
                    HttpClient client = new HttpClient();
                    //client.BaseAddress = new Uri(ConfigurationManager.AppSettings["StratisBlockChainBaseUrl"]);
                    var stratisApiComponent = moocMicroCredentialProvider.StratisApiFullfilRequestComponent = new UniSA.Services.StratisBlockChainServices.StratisApi.StratisApiFullfilRequestComponent(client);

                    var walletCreated = await stratisApiComponent.CreateWallet(stratisAccountModel.AccountWalletName, ConfigurationManager.AppSettings["StratisAccountFormationLatePassword"], stratisAccountModel.AccountName);
                    if (walletCreated)
                    {
                        var accountAddresses = await stratisApiComponent.GetAccountAddresses(stratisAccountModel.AccountWalletName, stratisAccountModel.AccountName);

                        try
                        {
                            if (stratisAccountModel.StratisAccountId < 1)
                            {
                                _unitOfWork.StratisAccountRepository.Add(new UniSA.Domain.StratisAccount
                                {
                                    AccountName = stratisAccountModel.AccountName,
                                    EmailAddress = stratisAccountModel.EmailAddress,
                                    AccountWalletName = stratisAccountModel.AccountWalletName,
                                    AccountStratisAddress1 = accountAddresses.addresses[0].address,
                                    AccountStratisAddress2 = accountAddresses.addresses[1].address,
                                    AccountStratisAddress3 = accountAddresses.addresses[2].address
                                });
                                _unitOfWork.SaveChanges();
                                return View("Success");
                            }
                            else
                            {
                                var stratisAccount = AutoMapperConfig.Configure().Map(stratisAccountModel, typeof(StratisAccountViewModel), typeof(StratisAccount)) as StratisAccount;
                                if (_unitOfWork.StratisAccountRepository.Update(stratisAccount))
                                {
                                    return View("Success");
                                }
                            }
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }

            }
            return View(stratisAccountModel);
        }
*/
    }

}
