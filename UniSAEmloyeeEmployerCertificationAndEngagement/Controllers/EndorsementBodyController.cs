using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RSA316Infinito.BigRsaCrypto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

    [Authorize(Roles = "EndorsementBody,Administrator")]
    public class EndorsementBodyController : Controller
    {
        UnitOfWork _unitOfWork;
        private RoleManager<IdentityRole> _roleManager;
        private RepositoryEndPointServices _repositoryEndPointService;

        public EndorsementBodyController(UnitOfWork unitOfWork)
        {
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            _unitOfWork = unitOfWork;
            _unitOfWork.UniSADbContext = new UniSA.DataAccess.UniSADbContext();
            var moocMicroCredentialProvider = new MoocMicroCredentialProvider(new StratisEndPointAdhocService(), new EncryptDecrypt());

            _repositoryEndPointService = new RepositoryEndPointServices(_unitOfWork, moocMicroCredentialProvider);            _repositoryEndPointService = new RepositoryEndPointServices(_unitOfWork, moocMicroCredentialProvider);
            moocMicroCredentialProvider.RepositoryEndPointService = _repositoryEndPointService;
        }
        private List<SelectListItem> GetMicroCredentialIds()
        {
            return _unitOfWork.MicroCredentialRepository.GetAll().Select(a => new SelectListItem { Text = a.MicroCredentialName, Value = a.MicroCredentialId.ToString() }).ToList();
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

        [HttpGet]
        public ActionResult Index(string AddminOtherRole)
        {
            ViewBag.AddminOtherRole = AddminOtherRole;
            return View();
        }

        [HttpGet]
        public ActionResult SelectEndorseMicroCredentialCourse()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.AccreditationBodyIdList = GetAccreditationBodyIds();
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
            return View(microCredentialViewModel);
        }

        [HttpPost]
        public ActionResult SelectEndorseMicroCredentialCourse(SelectDeleteMicroCredentialViewModel microCredentialViewModel)
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.AccreditationBodyIdList = GetAccreditationBodyIds();

            if (ModelState.IsValid)
            {
                ModelState.Clear();
                var selectedMicroCredential = _repositoryEndPointService.GetMicroCredentialById(microCredentialViewModel.MicroCredentialId);
                var selectedMicroCredentialViewModel = AutoMapperConfig.Configure().Map(selectedMicroCredential, typeof(MicroCredential), typeof(SelectDeleteMicroCredentialViewModel)) as SelectDeleteMicroCredentialViewModel;

                return View(selectedMicroCredentialViewModel);
            }
            return View(microCredentialViewModel);
        }

        [HttpGet]
        public ActionResult UpdateEndorseMicroCredentialCourse()
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.AccreditationBodyIdList = GetAccreditationBodyIds();

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
            return View(microCredentialViewModel);
        }

        [HttpPost]
        public ActionResult UpdateEndorseMicroCredentialCourse(MicroCredentialViewModel microCredentialViewModel)
        {
            ViewBag.AddressIdList = GetAddressIds();
            ViewBag.EndorsedBodyIdList = GetEndorsedBodyIds();
            ViewBag.MicroCredentialIdList = GetMicroCredentialIds();
            ViewBag.AccreditationBodyIdList = GetAccreditationBodyIds();

            if (ModelState.IsValid)
            {
                var microCredential = AutoMapperConfig.Configure().Map(microCredentialViewModel, typeof(MicroCredentialViewModel), typeof(MicroCredential)) as MicroCredential;
                _repositoryEndPointService.EndorseMicorCredential(microCredential);
                return View("Success");
            }
            return View(microCredentialViewModel);
        }
    }
}
