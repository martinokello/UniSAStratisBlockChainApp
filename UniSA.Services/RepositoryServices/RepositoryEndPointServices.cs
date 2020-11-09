using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using UniSA.Domain;
using UniSA.Services.UnitOfWork;
using System.Web.Mvc;
using UniSA.Services.StratisBlockChainServices;
using System.Configuration;
using UniSA.Services.StratisBlockChainServices.Providers;
using Newtonsoft.Json;
using UniSA.Services.StratisBlockChainServices.BlockChain.RequestModel;
using System.Threading.Tasks;

namespace UniSA.Services.RepositoryServices
{
    public class RepositoryEndPointServices
    {
        private UnitOfWork.UnitOfWork _unitOfWork;
        public MoocMicroCredentialProvider _moocMicroCredentialProvider;
        public RepositoryEndPointServices(UnitOfWork.UnitOfWork unitOfWork, MoocMicroCredentialProvider moocMicroCredentialProvider)
        {
            _unitOfWork = unitOfWork;
            _moocMicroCredentialProvider = moocMicroCredentialProvider;
        }
        public EmployerOperations EmployerOperations { get; set; }
        public EmployeeOperations EmployeeOperations { get; set; }
        public Dictionary<Candidate, List<SelectListItem>> GetCurrentUserAndCourses(string name)
        {
            var candidate = _unitOfWork.CandidateRepository.GetAll().FirstOrDefault(q => q.EmailAddress.ToLower().Equals(name.ToLower()));
            var courses = _unitOfWork.MicroCredentialRepository.GetAll().Select(q => new SelectListItem { Text = q.MicroCredentialName, Value = q.MicroCredentialId.ToString() }).ToList();

            var dict = new Dictionary<Candidate, List<SelectListItem>>();
            if(candidate != null && courses != null)
            dict.Add(candidate, courses);
            return dict;
        }

        public void EnroleUserToMicroCredentialCourse(int candidateId, int microCredentialId)
        {
            _unitOfWork.CandidateMicroCredentialCourseRepository.Add(new CandidateMicroCredentialCourse { CandidateId = candidateId, MicroCredentialId = microCredentialId });
            _unitOfWork.SaveChanges();
        }
        public void EnroleUserToMicroCredentialCourseUsingEmailId(string userEmail, int microCredentialId)
        {
            var candidate = _unitOfWork.CandidateRepository.GetAll().Where(c => c.EmailAddress.ToLower().Equals(userEmail.ToLower())).FirstOrDefault();
            if(candidate != null)
            {
                _unitOfWork.CandidateMicroCredentialCourseRepository.Add(new CandidateMicroCredentialCourse { CandidateId = candidate.CandidateId, MicroCredentialId = microCredentialId });
                _unitOfWork.SaveChanges();
            }
        }

        public AccreditationBody GetAccreditationBodyByEmail(string emailAddress)
        {
            return _unitOfWork.AccreditationBodyRepository.GetAll().FirstOrDefault(q => q.EmailAddress.ToLower().Equals(emailAddress.ToLower()));
        }

        public void UpdateAccreditationBody(AccreditationBody accreditationBody)
        {
            _unitOfWork.AccreditationBodyRepository.Update(accreditationBody);
            _unitOfWork.SaveChanges();
        }

        public CandidateMicroCredentialCourse GetCandidateAndCourseForVerification(int userId, int microCredentialId)
        {
            return _unitOfWork.CandidateMicroCredentialCourseRepository.GetAll().FirstOrDefault(q => q.HasPassed && q.MicroCredentialId == microCredentialId && q.CandidateId == userId);
        }

        public void InsertAccreditationBody(AccreditationBody accreditationBody)
        {
            _unitOfWork.AccreditationBodyRepository.Add(accreditationBody);
            _unitOfWork.SaveChanges();
        }

        public void DeleteAccreditationBody(AccreditationBody accreditationBody)
        {
            _unitOfWork.AccreditationBodyRepository.Add(accreditationBody);
            _unitOfWork.AccreditationBodyRepository.Delete(accreditationBody);
            _unitOfWork.SaveChanges();
        }

        public Candidate GetCandidateByEmail(string emailAddress)
        {
            return _unitOfWork.CandidateRepository.GetAll().FirstOrDefault(q => q.EmailAddress.ToLower().Equals(emailAddress.ToLower()));
        }

        public Address GetAddressById(int addressId)
        {
            return _unitOfWork.AddressRepository.GetById(addressId);
        }

        public void AddMicroCredentialCourse(MicroCredential microCredential)
        {
            _unitOfWork.MicroCredentialRepository.Add(microCredential);
            _unitOfWork.SaveChanges();
        }

        public void UpdateAddress(Address address)
        {
            _unitOfWork.AddressRepository.Update(address);
            _unitOfWork.SaveChanges();
        }

        public Job[] GetAllJobs()
        {
            return _unitOfWork.JobRepository.GetAll().ToArray();
        }

        public Job GetJobsById(int id)
        {
            return _unitOfWork.JobRepository.GetById(id);
        }

        public void InsertAddress(Address address)
        {
            _unitOfWork.AddressRepository.Add(address);
            _unitOfWork.SaveChanges();
        }

        public async Task<bool> CertifyCandidateUser(CandidateMicroCredentialCourse candidateMicroCredentialCourse)
        {
            var candidate = _unitOfWork.CandidateRepository.GetById(candidateMicroCredentialCourse.CandidateId);
            var microCredential = _unitOfWork.MicroCredentialRepository.GetById(candidateMicroCredentialCourse.MicroCredentialId);
            var moocProvider = _unitOfWork.MoocProviderRepository.GetAll().FirstOrDefault(m => m.MoocProviderId == microCredential.MoocProviderId);

            if (moocProvider == null) throw new Exception($"MoocProvider Cannot be null, for MicroCredentialId = {microCredential.MoocProviderId}");
            var stratisBlockDataToSign = _moocMicroCredentialProvider.GetUserDataToCertify(candidate.EmailAddress,moocProvider.EmailAddress, microCredential);
            var signature =_moocMicroCredentialProvider.CreateSignature(stratisBlockDataToSign);
            //_moocMicroCredentialProvider.StratisMineBlockThenReturnResponse(stratisBlockDataSigned);
            //var signBlockRequest = new SignBlockRequest { externalAddress = ConfigurationManager.AppSettings["ExternalAddress"], message = signature /*stratisBlockDataToSign*/, walletName = ConfigurationManager.AppSettings["StratisWalletName"], password = ConfigurationManager.AppSettings["StratisWalletAccountPassword"] };
            //var signature = _moocMicroCredentialProvider.StratisApiFullfilRequestComponent.SignBlockData(signBlockRequest).Result; 

            var transactionDataRequest = new TransactionBuildData
            {
                accountName = "account 0",
                allowUnconfirmed = true,
                shuffleOutputs = true,
                changeAddress = ConfigurationManager.AppSettings["StratisWalletAddress"],
                feeType = "medium",
                walletName = ConfigurationManager.AppSettings["StratisWalletName"],
                password = ConfigurationManager.AppSettings["StratisWalletAccountPassword"],
                recipients = new Recipient[] { new Recipient { amount = microCredential.CertificateFee.ToString(), destinationAddress = ConfigurationManager.AppSettings["ExternalAddress"] } }
            };
            var trasaction = await _moocMicroCredentialProvider.StratisApiFullfilRequestComponent.BuildTransaction(transactionDataRequest);

            var transactionResult = await _moocMicroCredentialProvider.StratisApiFullfilRequestComponent.SendTransaction(trasaction.hex);
            var candidateCourseCredential = _unitOfWork.CandidateMicroCredentialCourseRepository.GetAll().Where(c => c.MicroCredentialId == candidateMicroCredentialCourse.MicroCredentialId && c.CandidateId == candidateMicroCredentialCourse.CandidateId).FirstOrDefault();
            //Save to Hash to DB:
            if(candidateCourseCredential != null)
            {
                candidateCourseCredential.HasPassed = true;
                candidateCourseCredential.HashOfMine = signature;
                candidateCourseCredential.transactionHex = trasaction.hex;
                candidateCourseCredential.transactionId = trasaction.transactionId;
                candidateCourseCredential.MicroCredentialBadgeId = candidateMicroCredentialCourse.MicroCredentialBadgeId;
                _unitOfWork.CandidateMicroCredentialCourseRepository.Update(candidateCourseCredential);
                _unitOfWork.SaveChanges();
                return true;
            }
            return false;
        }

        public void InsertCandidate(Candidate candidate)
        {
            _unitOfWork.CandidateRepository.Add(candidate);
            _unitOfWork.SaveChanges();
        }

        public void UpdateCandidate(Candidate candidate)
        {
            _unitOfWork.CandidateRepository.Update(candidate);
            _unitOfWork.SaveChanges();
        }

        public void AddNewCandidateJobApplication(CandidateJobApplication jobApplication)
        {
            _unitOfWork.CandidateJobApplicationRepository.Add(jobApplication);
            _unitOfWork.SaveChanges();
        }

        public AccreditationBody GetAccreditationBodyById(int accreditationBodyId)
        {
            return _unitOfWork.AccreditationBodyRepository.GetById(accreditationBodyId);
        }

        public Candidate GetCandidateByEmailAddress(string emailAddress)
        {
            return _unitOfWork.CandidateRepository.GetAll().FirstOrDefault(c => c.EmailAddress.ToLower().Equals(emailAddress.ToLower()));
        }

        public void DeleteCandidate(Candidate candidate)
        {
            _unitOfWork.CandidateRepository.Delete(candidate);
            _unitOfWork.SaveChanges();
        }

        public void DeleteAddress(Address address)
        {

            _unitOfWork.AddressRepository.Delete(address);
            _unitOfWork.SaveChanges();
        }

        public void UpdateMoocProviderKeys(MoocProvider moocProvider)
        {
            //moocProvider is straight out of the database so update by saving changes:
            _unitOfWork.SaveChanges();
        }

        public MoocProvider GetMoocProviderFromDB()
        {
            var moocsEmailArray = ConfigurationManager.AppSettings["MoocProviderEmails"].Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries);
            Array.ForEach(moocsEmailArray, (e) =>
            {
                e = e.ToLower();
            });
            return _unitOfWork.MoocProviderRepository.GetAll().FirstOrDefault(m => moocsEmailArray.Contains(m.EmailAddress.ToLower()));
        }

        public Employer GetEmployerByName(string employerName)
        {
            return _unitOfWork.EmployerRepository.GetAll().FirstOrDefault(e => e.EmployerName.ToLower().Equals(employerName.ToLower()));
        }

        public Employer GetEmployerByEmail(string employerEmail)
        {
            return _unitOfWork.EmployerRepository.GetAll().FirstOrDefault(e => e.ContactEmailAddress.ToLower().Equals(employerEmail.ToLower()));
        }
        public void EndorseMicorCredential(MicroCredential microCredential)
        {
            var microCredentialObj = _unitOfWork.MicroCredentialRepository.GetById(microCredential.MicroCredentialId);
            microCredentialObj.IsEndorsed = true;
            _unitOfWork.SaveChanges();
        }

        public void UpdateEmployer(Employer employer)
        {
            _unitOfWork.EmployerRepository.Update(employer);
            _unitOfWork.SaveChanges();
        }

        public UserMicroCredentialBadges GetUserMicroCredentialBadgesById(int candidateId, int microCredentialId)
        {
            return _unitOfWork.CandidatetMicroCredentialBadgesRepository.GetAll().FirstOrDefault(q => q.CandidateId == candidateId && q.MicroCredentialId == microCredentialId);
        }

        public void InsertEmployer(Employer employer)
        {
            _unitOfWork.EmployerRepository.Add(employer);
            _unitOfWork.SaveChanges();
        }

        public void EmployerCreateJob(Job job)
        {
            _unitOfWork.JobRepository.Add(job);
            _unitOfWork.SaveChanges();
        }

        public void DeleteEmployer(Employer employer)
        {
            _unitOfWork.EmployerRepository.Delete(employer);
            _unitOfWork.SaveChanges();
        }

        public void EmployerUpdateJob(Job job)
        {
            var jobObj =_unitOfWork.JobRepository.GetById(job.JobId);
            jobObj.DateUpdated = job.DateUpdated;
            jobObj.IsActive = job.IsActive;
            jobObj.JobCode = job.JobCode;
            jobObj.JobTitle = job.JobTitle;
            jobObj.MicroCredentialRequired = job.MicroCredentialRequired;
            jobObj.QualificationsRequired = job.QualificationsRequired;
            _unitOfWork.SaveChanges();
        }

        public void UpdateJob(Job job)
        {
            _unitOfWork.JobRepository.Update(job);
            _unitOfWork.SaveChanges();
        }

        public Job[] GetJobByTitle(string jobTitle)
        {
            return _unitOfWork.JobRepository.GetAll().Where(j => j.JobTitle.Contains(jobTitle)).ToArray();
        }

        public void UpdateMicroCredentialsForJobId(int jobId, List<int> microCredentialIds)
        {
            var job = _unitOfWork.JobRepository.GetById(jobId);

            var microCredentials = _unitOfWork.MicroCredentialRepository.GetAll().Where(q => microCredentialIds.Contains(q.MicroCredentialId));
            
            foreach(var micCred in microCredentials)
            {
                micCred.JobId = jobId;
            }
            _unitOfWork.SaveChanges();
        }

        public EndorsementBody GetEndorsementBodyById(int endorsementBodyId)
        {
            return _unitOfWork.EndorsementBodyRepository.GetById(endorsementBodyId);
        }

        public MicroCredential[] GetJMicroCredentialByName(string microCredentialName)
        {
            return _unitOfWork.MicroCredentialRepository.GetAll().Where(m => m.MicroCredentialName.Contains(microCredentialName)).ToArray();
        }

        public void DeleteJob(Job job)
        {
            _unitOfWork.JobRepository.Delete(job);
            _unitOfWork.SaveChanges();
        }

        public void InsertJob(Job job)
        {
            _unitOfWork.JobRepository.Add(job);
            _unitOfWork.SaveChanges();
        }

        public void DeleteEndorsementBody(EndorsementBody endorsementBody)
        {
            endorsementBody = _unitOfWork.EndorsementBodyRepository.GetById(endorsementBody.EndorsementBodyId);
            _unitOfWork.EndorsementBodyRepository.Delete(endorsementBody);
            _unitOfWork.SaveChanges();
        }

        public void UpdateMicroCredential(MicroCredential microCredential)
        {
            _unitOfWork.MicroCredentialRepository.Update(microCredential);
            _unitOfWork.SaveChanges();
        }

        public bool VerifyUserBadges(UserMicroCredentialBadges userMicroCredential)
        {
            var microCredential = _unitOfWork.MicroCredentialRepository.GetById(userMicroCredential.MicroCredentialId);
            if (microCredential == null) return false;

            var usersMicroCredentialBadges2 = _unitOfWork.CandidateMicroCredentialCourseRepository.GetAll().FirstOrDefault(q => q.CandidateId == userMicroCredential.CandidateId && q.HasPassed /*&& q.MicroCredential.DigitalBadge.ToLower().Equals(userMicroCredential.MicroCredentialBadges.ToLower())*/); 
            if (usersMicroCredentialBadges2 == null)
            {
                return false;
            }
            //Verify Signature In Stratis BlockChain
            //StratisBlockData hashedBlock = _moocMicroCredentialProvider.StratisEndPointService.GetBlockWithHashFromChain();
            return _moocMicroCredentialProvider.VerifyUserCert(userMicroCredential.Username, microCredential);            
        }

        public void InsertEndorsementBody(EndorsementBody endorsementBody)
        {
            _unitOfWork.EndorsementBodyRepository.Add(endorsementBody);
            _unitOfWork.SaveChanges();
        }

        public void UpdateEndorsementBody(EndorsementBody endorsementBody)
        {
            _unitOfWork.EndorsementBodyRepository.Update(endorsementBody);
            _unitOfWork.SaveChanges();
        }

        public void DeleteMicroCredential(MicroCredential microCredential)
        {
            _unitOfWork.MicroCredentialRepository.Delete(microCredential);
            _unitOfWork.SaveChanges();
        }

        public MoocProvider GetMoocProviderByEmail(string moocProviderEmail)
        {
            return _unitOfWork.MoocProviderRepository.GetAll().FirstOrDefault(m => m.EmailAddress.ToLower().Equals(moocProviderEmail.ToLower())); 
        }
        public void InsertMicroCredential(MicroCredential microCredential)
        {
            _unitOfWork.MicroCredentialRepository.Add(microCredential);
            _unitOfWork.SaveChanges();
        }

        public List<MicroCredential> GetAllMicroCredentials()
        {
            return _unitOfWork.MicroCredentialRepository.GetAll().ToList();
        }

        public void UpdateMoocProvider(MoocProvider moocProvider)
        {
            _unitOfWork.MoocProviderRepository.Update(moocProvider);
            _unitOfWork.SaveChanges();
        }

        public void DeleteJobs(int employerId, int jobId)
        {
            var job =_unitOfWork.JobRepository.GetAll().Where(j=>j.JobId == jobId).FirstOrDefault();
            if (job != null)
            {
                _unitOfWork.JobRepository.Delete(job);
                _unitOfWork.SaveChanges();
            }
        }

        public void InsertMoocProvider(MoocProvider moocProvider)
        {
            _unitOfWork.MoocProviderRepository.Add(moocProvider);
            _unitOfWork.SaveChanges();
        }

        public void DeleteMoocProvider(MoocProvider moocProvider)
        {
            _unitOfWork.MoocProviderRepository.Delete(moocProvider);
            _unitOfWork.SaveChanges();
        }

        public Job[] GetJobsByEmployerId(int employerId)
        {
            return _unitOfWork.JobRepository.GetAll().Where(e => e.EmployerId == employerId).ToArray();
        }

        public void AccreditMicroCredential(MicroCredential microCredential)
        {
            var microCredentialObj = _unitOfWork.MicroCredentialRepository.GetById(microCredential.MicroCredentialId);
            microCredentialObj.IsAccredited = true;
            _unitOfWork.SaveChanges();
        }

        public List<UserMicroCredentialBadges> GetCandidateDigitalBadgesByEmail(string candidateEmail)
        {
            var badgesList = new List<UserMicroCredentialBadges>();
            var candidate = _unitOfWork.CandidateRepository.GetAll().FirstOrDefault(c => c.EmailAddress.ToLower().Equals(candidateEmail.ToLower()));
            if(candidate != null)
            {
                var userMicroCredBadges = _unitOfWork.CandidatetMicroCredentialBadgesRepository.GetAll().Where(q => q.CandidateId == candidate.CandidateId).
                    Select(s => new { CandidateId = s.CandidateId, Username = candidate.UserName,
                        MicroCredentialBadges = s.MicroCredentialBadges, MicroCredentialBadgeId = s.MicroCredentialBadgeId, MicroCredentialId = s.MicroCredentialId
                    }).ToList();

                foreach (var el in userMicroCredBadges)
                {
                    badgesList.Add(new UserMicroCredentialBadges
                    {
                        CandidateId = el.CandidateId,
                        MicroCredentialBadges = el.MicroCredentialBadges,
                        Username = el.Username,
                        MicroCredentialBadgeId = el.MicroCredentialBadgeId,
                        MicroCredentialId = el.MicroCredentialId
                    });
                }
            }
            return badgesList;
        }

        public RecruitmentAgency[] GetRecruitmentAgencyByName(string recruitmentAgencyName)
        {
            return _unitOfWork.RecruitmentAgencyRepository.GetAll().Where(r => r.RecruitmentAgencyName.Contains(recruitmentAgencyName)).ToArray();
        }

        public void UpdatetRecruitmentAgency(RecruitmentAgency recruitmentAgency)
        {
            _unitOfWork.RecruitmentAgencyRepository.Update(recruitmentAgency);
            _unitOfWork.SaveChanges();
        }

        public Candidate GetCandidateById(int userId)
        {
            return _unitOfWork.CandidateRepository.GetById(userId);
        }

        public void InsertRecruitmentAgency(RecruitmentAgency recruitmentAgency)
        {
                _unitOfWork.RecruitmentAgencyRepository.Add(recruitmentAgency);
                _unitOfWork.SaveChanges();
        }

        public void DeletetRecruitmentAgency(RecruitmentAgency recruitmentAgency)
        {
            _unitOfWork.RecruitmentAgencyRepository.Delete(recruitmentAgency);
            _unitOfWork.SaveChanges();
        }

        public Employer GetEmployerById(int employerId)
        {
            return _unitOfWork.EmployerRepository.GetById(employerId);
        }

        public Job GetJobById(int jobId)
        {
            return _unitOfWork.JobRepository.GetById(jobId);
        }

        public MicroCredential GetMicroCredentialById(int microCredentialId)
        {
            return _unitOfWork.MicroCredentialRepository.GetById(microCredentialId);
        }

        public MoocProvider GetMoocProviderById(int moocProviderId)
        {
            return _unitOfWork.MoocProviderRepository.GetById(moocProviderId);
        }

        public RecruitmentAgency GetRecruitmentAgencyById(int recruitmentAgencyId)
        {
            return _unitOfWork.RecruitmentAgencyRepository.GetById(recruitmentAgencyId);
        }

        public UserMicroCredentialBadges[] GetUserMicroCredentialBadgesById(int candidateId)
        {
            return _unitOfWork.CandidatetMicroCredentialBadgesRepository.GetAll().Where(cmb=> cmb.CandidateId == candidateId).ToArray();
        }

        public UserMicroCredentialBadges GetUserMicroCredentialBadgesByBadgeId(int microCredentialBadgeId)
        {
            return _unitOfWork.CandidatetMicroCredentialBadgesRepository.GetById(microCredentialBadgeId);
        }
    }
}
