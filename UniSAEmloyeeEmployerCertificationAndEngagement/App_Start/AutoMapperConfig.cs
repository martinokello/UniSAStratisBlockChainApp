using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using UniSA.Domain;
using UniSAEmloyeeEmployerCertificationAndEngagement.Models;

namespace UniSAEmloyeeEmployerCertificationAndEngagement.App_Start
{
    public class AutoMapperConfig
    {
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MicroCredential, MicroCredentialViewModel>();
                cfg.CreateMap<MicroCredential, MicroCredentialViewModel>().ReverseMap();

                cfg.CreateMap<MicroCredential, SelectDeleteMicroCredentialViewModel>();
                cfg.CreateMap<MicroCredential, SelectDeleteMicroCredentialViewModel>().ReverseMap();
                cfg.CreateMap<CandidateMicroCredentialCourse, CandidateMicroCredentialCourseViewModel>();
                cfg.CreateMap<CandidateMicroCredentialCourse, CandidateMicroCredentialCourseViewModel>().ReverseMap();
                cfg.CreateMap<AccreditationBody, AccreditationBodyViewModel>();
                cfg.CreateMap<AccreditationBody, AccreditationBodyViewModel>().ReverseMap();
                cfg.CreateMap<AccreditationBody, SelectDeleteAccreditationBodyViewModel>();
                cfg.CreateMap<AccreditationBody, SelectDeleteAccreditationBodyViewModel>().ReverseMap();
                cfg.CreateMap<Address, AddressViewModel>();
                cfg.CreateMap<Address, AddressViewModel>().ReverseMap();
                cfg.CreateMap<Address, SelectDeleteAddressViewModel>();
                cfg.CreateMap<Address, SelectDeleteAddressViewModel>().ReverseMap();
                cfg.CreateMap<Candidate, CandidateViewModel> ();
                cfg.CreateMap<Candidate, CandidateViewModel>().ReverseMap();
                cfg.CreateMap<Candidate, SelectDeleteCandidateViewModel>();
                cfg.CreateMap<Candidate, SelectDeleteCandidateViewModel>().ReverseMap();
                cfg.CreateMap<Employer, EmployerViewModel>();
                cfg.CreateMap<Employer, EmployerViewModel>().ReverseMap();
                cfg.CreateMap<Employer, SelectDeleteEmployerViewModel>();
                cfg.CreateMap<Employer, SelectDeleteEmployerViewModel>().ReverseMap();
                cfg.CreateMap<EndorsementBody, EndorsementBodyViewModel>();
                cfg.CreateMap<EndorsementBody, EndorsementBodyViewModel>().ReverseMap();
                cfg.CreateMap<EndorsementBody, SelectDeleteEndorsementBodyViewModel>();
                cfg.CreateMap<EndorsementBody, SelectDeleteEndorsementBodyViewModel>().ReverseMap();
                cfg.CreateMap<Government, GovernmentViewModel>();
                cfg.CreateMap<Government, GovernmentViewModel>().ReverseMap();
                cfg.CreateMap<Government, SelectDeleteGovernmentViewModel>();
                cfg.CreateMap<Government, SelectDeleteGovernmentViewModel>().ReverseMap();
                cfg.CreateMap<Job, JobViewModel>();
                cfg.CreateMap<Job, JobViewModel>().ReverseMap();
                cfg.CreateMap<Job, SelectDeleteJobViewModel>();
                cfg.CreateMap<Job, SelectDeleteJobViewModel>().ReverseMap(); 
                cfg.CreateMap<UserMicroCredentialBadges, ValidateUserMicroCredentialBadges>();
                cfg.CreateMap<UserMicroCredentialBadges, ValidateUserMicroCredentialBadges>().ReverseMap();
                cfg.CreateMap<UserMicroCredentialBadges, UserMicroCredentialBadgesViewModel>(); 
                cfg.CreateMap<UserMicroCredentialBadges, UserMicroCredentialBadgesViewModel>().ReverseMap();
                cfg.CreateMap<UserMicroCredentialBadges, SelectDeleteMicroCredentialUserViewModel>();
                cfg.CreateMap<UserMicroCredentialBadges, SelectDeleteMicroCredentialUserViewModel>().ReverseMap();
                cfg.CreateMap<UserMicroCredentialBadges, SelectDeleteMicroCredentialBadgeViewModel>();
                cfg.CreateMap<UserMicroCredentialBadges, SelectDeleteMicroCredentialBadgeViewModel>().ReverseMap();

                cfg.CreateMap<UserMicroCredentialBadges, MicroCredentialBadgeViewModel>();
                cfg.CreateMap<MicroCredentialBadgeViewModel, UserMicroCredentialBadges>().ReverseMap();
                cfg.CreateMap<MoocProvider, MoocProviderViewModel>();
                cfg.CreateMap<MoocProvider, MoocProviderViewModel>().ReverseMap();
                cfg.CreateMap<MoocProvider, SelectDeleteMoocProviderViewModel>();
                cfg.CreateMap<MoocProvider, SelectDeleteMoocProviderViewModel>().ReverseMap();
                cfg.CreateMap<RecruitmentAgency, RecruitmentAgentViewModel>();
                cfg.CreateMap<RecruitmentAgency, RecruitmentAgentViewModel>().ReverseMap();
                cfg.CreateMap<RecruitmentAgency, SelectDeleteRecruitmentAgentViewModel>();
                cfg.CreateMap<RecruitmentAgency, SelectDeleteRecruitmentAgentViewModel>().ReverseMap(); 
                cfg.CreateMap<CandidateJobApplicationViewModel, CandidateJobApplication>();
                cfg.CreateMap<CandidateJobApplication, CandidateJobApplicationViewModel>().ReverseMap();
                cfg.CreateMap<StratisAccountViewModel, StratisAccount>();
                cfg.CreateMap<StratisAccountViewModel, StratisAccount>().ReverseMap();
                cfg.CreateMap<SelectStratisAccountViewModel, StratisAccount>();
                cfg.CreateMap<SelectStratisAccountViewModel, StratisAccount>().ReverseMap();
            });
            return config.CreateMapper();
        }
    }
}