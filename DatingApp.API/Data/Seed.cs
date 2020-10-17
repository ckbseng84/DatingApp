using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public static class Seed
    {
        public static void SeedUsers(UserManager<User> userManager,RoleManager<Role> roleManager){
            if (!userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                
                // create some roles
                var roles = new List<Role>
                {
                    new Role{Name = "Member"},
                    new Role{Name = "Admin"},
                    new Role{Name = "Moderator"},
                    new Role{Name = "VIP"}   
                };
                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }
                foreach (var user in users)
                {
                    //TODO no hardcode, create another object to store user from seed
                    //then call mapper to map and assign the password
                    userManager.CreateAsync(user,"password").Wait();
                    userManager.AddToRoleAsync(user,"Member").Wait();
                    //have to comment, otherwise got error
                    //user.Photos.SingleOrDefault().IsApproved = true;
                }

                var adminUser = new User
                {
                    UserName = "Admin"
                };
                var result = userManager.CreateAsync(adminUser,"password").Result;

                if(result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    userManager.AddToRolesAsync(admin, new [] {"Admin","Moderator"});

                }

                
            }

        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //hmac will be disposed once out of "using"
            //this to ensure key would not store in memory 
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
    }
}