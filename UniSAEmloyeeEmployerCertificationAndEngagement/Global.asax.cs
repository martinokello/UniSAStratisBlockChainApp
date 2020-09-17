using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RSA316Infinito.BigRsaCrypto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using UniSA.Services.StratisBlockChainServices;
using UniSA.Services.StratisBlockChainServices.Providers;
using UniSAEmloyeeEmployerCertificationAndEngagement.App_Start;
using UniSAEmloyeeEmployerCertificationAndEngagement.Models;

namespace UniSAEmloyeeEmployerCertificationAndEngagement
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            AutofacConfig.RegisterTypes();
            AutoMapperConfig.Configure();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Roles Register if not registered:

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            if (!roleManager.RoleExists("Administrator"))
            {
                roleManager.Create(new IdentityRole("Administrator"));
            }
            if (!roleManager.RoleExists("MoocProvider"))
            {
                roleManager.Create(new IdentityRole("MoocProvider"));
            }
            if (!roleManager.RoleExists("Employer"))
            {
                roleManager.Create(new IdentityRole("Employer"));
            }
            if (!roleManager.RoleExists("Candidate"))
            {
                roleManager.Create(new IdentityRole("Candidate"));
            }
            
            if (!roleManager.RoleExists("EndorsementBody"))
            {
                roleManager.Create(new IdentityRole("EndorsementBody"));
            }
            if (!roleManager.RoleExists("AccreditationBody"))
            {
                roleManager.Create(new IdentityRole("AccreditationBody"));
            }
            if (!roleManager.RoleExists("RecruitmentAgency"))
            {
                roleManager.Create(new IdentityRole("RecruitmentAgency"));
            }
            if (!roleManager.RoleExists("Government"))
            {
                roleManager.Create(new IdentityRole("Government"));
            }

            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var administratorAppUser = new ApplicationUser();
            var adminEmail = "administrator@unisa.ac.za";
            var adminUser = userManager.FindByEmail(adminEmail);

            if (adminUser == null)
            {
                administratorAppUser.Email = adminEmail;
                administratorAppUser.EmailConfirmed = true;
                administratorAppUser.LockoutEnabled = false;
                administratorAppUser.UserName = adminEmail;

                userManager.Create(administratorAppUser, "d3lt4X!505");
            }
            var adminRole = roleManager.FindByName("Administrator");

            adminUser = userManager.FindByEmail("administrator@unisa.ac.za");
            if(adminUser != null)
            {
                var idUserRole = new IdentityUserRole();
                idUserRole.UserId = adminUser.Id;
                idUserRole.RoleId = adminRole.Id;

                if (!adminUser.Roles.Contains(idUserRole))
                {
                    userManager.AddToRole(adminUser.Id, "Administrator");
                }
            }
            /*
            var moocMicroCredentialProvider = new MoocMicroCredentialProvider(new StratisEndPointAdhocService(), new EncryptDecrypt());
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["StratisBlockChainBaseUrl"]);
            moocMicroCredentialProvider.StratisApiFullfilRequestComponent = new UniSA.Services.StratisBlockChainServices.StratisApi.StratisApiFullfilRequestComponent(client);
            if (!moocMicroCredentialProvider.StratisApiFullfilRequestComponent.LoadWallet(ConfigurationManager.AppSettings["StratisWalletName"], ConfigurationManager.AppSettings["StratisWalletAccountPassword"]).Result)
            {
                throw (new Exception($"Stratis Wallet for Server:{client.BaseAddress} wasn't loaded!!"));
            }
            
            if (!moocMicroCredentialProvider.StratisApiFullfilRequestComponent.BeginStaking(ConfigurationManager.AppSettings["StratisWalletName"],ConfigurationManager.AppSettings["StratisWalletAccountPassword"]).ConfigureAwait(true).GetAwaiter().GetResult()) 
            {
                throw (new Exception("Stratis Block Chain Not set up For Staking nor Mining!!" ));
            }
         
            var stratisAccountBalanceAtAddress = moocMicroCredentialProvider.StratisApiFullfilRequestComponent.GetSpendableAmountAtWalletAddress(ConfigurationManager.AppSettings["StratisWalletAddress"]).Result;

            var contentInitialBalance = $"walletAddress:{stratisAccountBalanceAtAddress.address}\nSpendableAmount {stratisAccountBalanceAtAddress.spendableAmount}";
            Console.WriteLine(contentInitialBalance);
             */
        }
    }
}
