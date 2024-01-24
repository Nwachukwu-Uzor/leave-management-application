using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.LeaveManagement.Identity.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var hasher = new PasswordHasher<ApplicationUser>();
        builder.HasData(
            new ApplicationUser
            {
                Id = "ba576318-bbf7-46ea-9996-0c853926125e",
                FirstName = "System",
                LastName = "Admin",
                Email = "Admin@mail.com",
                NormalizedEmail = "ADMIN@MAIL.COM",
                PasswordHash = hasher.HashPassword(null, "12345"),
                EmailConfirmed = true,
            },
            new ApplicationUser
            {
                Id = "33724bca-9882-4a55-88ef-db41bacb9841",
                FirstName = "System",
                LastName = "User",
                Email = "user@mail.com",
                NormalizedEmail = "USER@MAIL.COM",
                PasswordHash = hasher.HashPassword(null, "12345"),
                EmailConfirmed = true,
            }
        );
    }
}
