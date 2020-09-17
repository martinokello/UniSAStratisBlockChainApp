using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniSA.Services.UnitOfWork;
using UniSAEmloyeeEmployerCertificationAndEngagement.Models;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.Controllers
{
    [Authorize(Roles ="Government,Administrator")]
    public class GovernmentController : Controller
    {
        UnitOfWork _unitOfWork;
        private RoleManager<IdentityRole> _roleManager;

        public GovernmentController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.UniSADbContext = new UniSA.DataAccess.UniSADbContext();
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
        // GET: Government
        [HttpGet]
        public ActionResult Index(string AddminOtherRole)
        {
            ViewBag.AddminOtherRole = AddminOtherRole;
            return View();
        }

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
        // GET: Government/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Government/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Government/Create
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

        // GET: Government/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Government/Edit/5
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

        // GET: Government/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Government/Delete/5
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
