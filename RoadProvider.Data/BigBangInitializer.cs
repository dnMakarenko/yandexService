using RoadProvider.Core.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using RoadProvider.DataImporter;

namespace RoadProvider.DataImporter
{
    public class BigBangInitializer : DropCreateDatabaseIfModelChanges<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            Initialize(context);
            base.Seed(context);
        }

        public void Initialize(AppDbContext context)
        {
            try
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
                {
                    AllowOnlyAlphanumericUserNames = false
                };
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                if (!roleManager.RoleExists("Admin"))
                {
                    roleManager.Create(new IdentityRole("Admin"));
                }

                if (!roleManager.RoleExists("Member"))
                {
                    roleManager.Create(new IdentityRole("Member"));
                }

                var user = new ApplicationUser()
                {
                    Email = "user@user.user",
                    UserName = "user@user.user",
                    FirstName = "User",
                    LastName = "User"
                };

                var userResult = userManager.Create(user, "admin");

                if (userResult.Succeeded)
                {
                    userManager.AddToRole<ApplicationUser, string>(user.Id, "Admin");
                }

                ImportRegions(context);

                context.Commit();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ImportRegions(AppDbContext context)
        {
            var regions = RegionRepository.RegionCodes;
            if (regions != null)
            {
                foreach (var region in regions)
                {
                    context.Regions.Add(region);
                }
            }
        }
    }
}
