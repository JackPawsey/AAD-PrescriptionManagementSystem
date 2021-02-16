using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AADWebApp.Areas.Identity.Data
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed initial roles for the system
            builder.Entity<IdentityRole>().HasData(new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "a0abf262-1a77-4b9d-bac5-ec293928f9ae",
                    Name = "Pharmacist",
                    NormalizedName = "PHARMACIST".ToUpper()
                },
                new IdentityRole
                {
                    Id = "5cf92bcd-61c7-40be-bf40-857cd7e94679",
                    Name = "Technician",
                    NormalizedName = "TECHNICIAN".ToUpper()
                },
                new IdentityRole
                {
                    Id = "dac4ae7a-4b01-4865-8f3d-66e4cb0bdb42",
                    Name = "General Practitioner",
                    NormalizedName = "GENERAL PRACTITIONER".ToUpper()
                },
                new IdentityRole
                {
                    Id = "7bdb12d3-caf8-4d43-a2e9-ef6ebe8f4b31",
                    Name = "Admin",
                    NormalizedName = "ADMIN".ToUpper()
                },
                new IdentityRole
                {
                    Id = "89363d4b-e187-4c02-8959-c3fa597d0846",
                    Name = "Patient",
                    NormalizedName = "PATIENT".ToUpper()
                },
                new IdentityRole
                {
                    Id = "4d2715ee-88a0-4631-8339-cf24311bafbc",
                    Name = "Authorised Carer",
                    NormalizedName = "AUTHORISED CARER".ToUpper()
                }
            });

            // Harsher to hash password for default administrator user creation
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            addUser("be2497f5-ab1f-4824-9a94-a14747bcccd7", "cloudcrusaderssystems@gmail.com", "CloudCrusaders@2021", "7bdb12d3-caf8-4d43-a2e9-ef6ebe8f4b31", builder, passwordHasher);
            addUser("fd064d4e-7457-4287-a3f4-5b99580ef2ab", "cloudcrusaderssystems+pharmacist@gmail.com", "CloudCrusaders@2021", "a0abf262-1a77-4b9d-bac5-ec293928f9ae", builder, passwordHasher);
            addUser("01734a51-05b1-4c95-8d21-6820014332e9", "cloudcrusaderssystems+technician@gmail.com", "CloudCrusaders@2021", "5cf92bcd-61c7-40be-bf40-857cd7e94679", builder, passwordHasher);
            addUser("c299b237-a197-454d-b474-587e7fe61656", "cloudcrusaderssystems+general.practitioner@gmail.com", "CloudCrusaders@2021", "dac4ae7a-4b01-4865-8f3d-66e4cb0bdb42", builder, passwordHasher);
            addUser("250f3fea-59bd-4f65-ba6a-a08b7afad55a", "cloudcrusaderssystems+patient@gmail.com", "CloudCrusaders@2021", "89363d4b-e187-4c02-8959-c3fa597d0846", builder, passwordHasher);
            addUser("33a728ad-f9f0-414b-a0d7-4d3cda8dbd6b", "cloudcrusaderssystems+authorised.carer@gmail.com", "CloudCrusaders@2021", "4d2715ee-88a0-4631-8339-cf24311bafbc", builder, passwordHasher);
        }

        private void addUser(string id, string email, string password, string targetRoleId, ModelBuilder builder, PasswordHasher<ApplicationUser> passwordHasher)
        {
            // Add Admin user with default password to users table
            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = id,
                    UserName = email.ToLower(),
                    NormalizedUserName = email.ToUpper(),
                    PasswordHash = passwordHasher.HashPassword(null, password),
                    Email = email.ToLower(),
                    NormalizedEmail = email.ToUpper()
                }
            );

            // Add the above user to the "Admin" role
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = targetRoleId,
                    UserId = id
                }
            );
        }
    }
}