// using System.Security.Cryptography;
// using System.Text;
// using System.Text.Json;
// using API.Entities;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;

// namespace API.Data;

// public class Seed
// {
//     public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
//     {
//         if (await userManager.Users.AnyAsync()) return;

//         var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

//         var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

//         var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

//         if (users == null) return;

//         var roles = new List<AppRole>
//         {
//             new() {Name = "Member"},
//             new() {Name = "Admin"},
//             new() {Name = "Moderator"},
//         };

//         foreach (var role in roles)
//         {
//           //  await roleManager.CreateAsync(role);
//         }

//         foreach (var user in users)
//         {
//             user.Photos.First().IsApproved = true;
//             user.UserName = user.UserName!.ToLower();
//             await userManager.CreateAsync(user, "Pa$$w0rd");
//             await userManager.AddToRoleAsync(user, "Member");
//         }

//         var admin = new AppUser
//         {
//             UserName = "admin",
//             KnownAs = "Admin",
//             Gender = "",
//             City = "",
//             Country = ""
//         };

//         await userManager.CreateAsync(admin, "Pa$$w0rd");
//         await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);
//     }
// }




 using System.Security.Cryptography;
 using System.Text;
 using System.Text.Json;
 using API.Entities;
 using Microsoft.AspNetCore.Identity;
 using Microsoft.EntityFrameworkCore;

 namespace API.Data;

 public class Seed
 {
     public static async Task SeedUsers(UserManager<AppUser> userManager,
      RoleManager<AppRole> roleManager)//DataContext data)
     {
         if (await userManager.Users.AnyAsync()) return;

         var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

         var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

         var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users == null) return;

        var roles = new List<AppRole>
        {
            new() {Name = "Member"},
            new() {Name = "Admin"},
            new() {Name = "Moderator"},
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        //using var hmac = new HMACSHA512();
       foreach (var user in users)
         {
             user.Photos.First().IsApproved = true;
             user.UserName = user.UserName!.ToLower();
           // user.PasswordHashed= hmac.ComputeHash(Encoding.UTF8.GetBytes("1234"));
             //user.PasswordSalt = hmac.Key;

           //data.Users.Add(user);
           user.UserName=user.UserName.ToLower();

           await userManager.CreateAsync(user,"Pa$$w0rd");
           await userManager.AddToRoleAsync(user, "Member");
        }


         //await data.SaveChangesAsync();
        var admin = new AppUser
        {
            UserName = "admin",
            KnownAs = "Admin",
            Gender = "",
            City = "",
            Country = ""
        };

        await userManager.CreateAsync(admin, "Pa$$w0rd");
        await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);
     }
 }