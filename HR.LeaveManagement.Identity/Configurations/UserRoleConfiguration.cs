using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.LeaveManagement.Identity.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{

    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(
            new IdentityUserRole<string>
            {
                RoleId = "8707ee99-0cff-45dc-98c3-50acff3b88b4",
                UserId = "ba576318-bbf7-46ea-9996-0c853926125e"
            },
            new IdentityUserRole<string>
            {
                RoleId = "b5a32d8b-ddfb-4a27-98de-5fdf9cb48c82",
                UserId = "33724bca-9882-4a55-88ef-db41bacb9841"
            }
        );
    }
}
