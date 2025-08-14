using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Concesionario.Domain.Entities.Core;
using Concesionario.Data.Identity;

namespace Concesionario.Data
{
    public class AuthenticateContext : IdentityDbContext
    {
        public AuthenticateContext(DbContextOptions<AuthenticateContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserIdentity>(b => { b.ToTable("UsersIdentity"); });
            builder.Entity<IdentityRole>(b => { b.ToTable("Roles"); });
            builder.Entity<IdentityUserRole<string>>(b => { b.ToTable("UsersRoles"); });
            builder.Entity<IdentityUserClaim<string>>(b => { b.ToTable("UsersClaims"); });
            builder.Entity<IdentityUserLogin<string>>(b => { b.ToTable("UsersLogins"); });
            builder.Entity<IdentityRoleClaim<string>>(b => { b.ToTable("RolesClaims"); });
            builder.Entity<IdentityUserToken<string>>(b => { b.ToTable("UsersTokens"); });

            
        }
    }
}
