using APYROPROJECTFINAL.Contants;
using Microsoft.AspNetCore.Identity;

namespace APYROPROJECTFINAL.Areas.Identity.Data
{
    public class DBSeeder
    {

        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<ApplicationUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();
            await roleManager.CreateAsync(new IdentityRole(Roles.Educator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Developer.ToString()));


            var user = new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@admin",
                //Name = "admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true

            };
            var userInDb = await userManager.FindByEmailAsync(user.Email);
            if(userInDb == null) 
            {
                await userManager.CreateAsync(user,"Admin@123");
                await userManager.AddToRoleAsync(user,Roles.Developer.ToString());
            }

        }

    


    }
}
