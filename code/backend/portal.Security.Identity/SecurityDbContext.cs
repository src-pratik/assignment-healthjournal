using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace portal.Security.Identity
{
    public class SecurityDbContext : IdentityDbContext<IdentityUser>
    {
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options)
        {
        }
    }
}