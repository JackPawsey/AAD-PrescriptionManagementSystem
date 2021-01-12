using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AADWebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AADWebApp.Data
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
            var hasher = new PasswordHasher<ApplicationUser>();

            // Add Admin user with default password to users table
            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "be2497f5-ab1f-4824-9a94-a14747bcccd7",
                    UserName = "cloudcrusaderssystems@gmail.com",
                    NormalizedUserName = "CLOUDCRUSADERSSYSTEMS@GMAIL.COM".ToUpper(),
                    PasswordHash = hasher.HashPassword(null, "CloudCrusaders@2021"),
                    Email = "cloudcrusaderssystems@gmail.com",
                    NormalizedEmail = "CLOUDCRUSADERSSYSTEMS@GMAIL.COM".ToUpper()
                }
            );

            // Add the above user to the "Admin" role
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "7bdb12d3-caf8-4d43-a2e9-ef6ebe8f4b31",
                    UserId = "be2497f5-ab1f-4824-9a94-a14747bcccd7"
                }
            );
        }
    }
}