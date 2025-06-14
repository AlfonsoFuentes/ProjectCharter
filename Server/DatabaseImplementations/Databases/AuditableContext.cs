using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Database.Identity;

namespace Server.DatabaseImplementations.Databases
{
    public abstract class AuditableContext : IdentityDbContext<BlazorHeroUser, IdentityRole, string, IdentityUserClaim<string>,
        IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        protected AuditableContext(DbContextOptions options) : base(options)
        {
        }




    }
}