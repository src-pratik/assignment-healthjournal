using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portal.Security.Identity
{
    public static class IdentitySeed
    {
        public static string[] roles = { "Admin", "User", "Publisher" };
        public static string[] users = { "user1@portal.com", "user2@portal.com" };
        public static string[] publishers = { "pub1@portal.com", "pub2@portal.com" };
        public static string password = "P@$$w0rd";

        public static void Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var item in roles)
            {
                IdentityRole searchRole = roleManager.FindByNameAsync(item).Result;
                if (searchRole != null)
                    continue;
                var role = new IdentityRole()
                {
                    Name = item,
                };

                _ = roleManager.CreateAsync(role).Result;
            }

            foreach (var item in users)
            {
                IdentityUser searchUser = userManager.FindByEmailAsync(item).Result;
                if (searchUser != null)
                    continue;

                IdentityUser user = new()
                {
                    Email = item,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = item
                };
                _ = userManager.CreateAsync(user, password).Result;
                searchUser = userManager.FindByEmailAsync(item).Result;

                _ = userManager.AddToRoleAsync(searchUser, "User").Result;
            }

            foreach (var item in publishers)
            {
                IdentityUser searchUser = userManager.FindByEmailAsync(item).Result;
                if (searchUser != null)
                    continue;

                IdentityUser user = new()
                {
                    Email = item,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = item
                };
                _ = userManager.CreateAsync(user, password).Result;
                searchUser = userManager.FindByEmailAsync(item).Result;

                _ = userManager.AddToRoleAsync(searchUser, "Publisher").Result;
            }
        }
    }
}