using System;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using UniSA.DataAccess.Concretes;
using UniSA.Services.UnitOfWork;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using UniSAEmloyeeEmployerCertificationAndEngagement.Models;
using UniSA.Services.StratisBlockChainServices.StratisApi;

namespace UniSAEmloyeeEmployerCertificationAndEngagement
{
    public class AutofacConfig
    {
        public static IContainer Build()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance<RoleManager<IdentityRole>>(new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext())));
            builder.RegisterInstance<ApplicationUserManager>(new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext())));
            builder.RegisterType<CandidateMicroCredentialCourseRepository>();
            builder.RegisterType<AddressRepository>();
            builder.RegisterType<CandidateRepository>();
            builder.RegisterType<EmployerRepository>();
            builder.RegisterType<GorvernmentRepository>();
            builder.RegisterType<JobRepository>();
            builder.RegisterType<MicroCredentialRepository>();
            builder.RegisterType<MoocProviderRepository>();
            builder.RegisterType<RecruitmentAgencyRepository>();
            builder.RegisterType<AccreditationBodyRepository>(); 
            builder.RegisterType<EndorsementBodyRepository>(); 
            builder.RegisterType<CandidateJobApplicationRepository>(); 
            builder.RegisterType<CandidatetMicroCredentialBadgesRepository>();
            builder.RegisterType<CandidatetMicroCredentialBadgesRepository>();
            builder.RegisterType<StratisAccountRepository>(); 
            builder.RegisterType<UnitOfWork>();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()); //Register MVC Controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            return builder.Build();
        }

        public static void RegisterTypes()
        {
            var container = AutofacConfig.Build();


            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}