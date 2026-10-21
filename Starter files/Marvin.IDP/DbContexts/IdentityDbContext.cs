    ﻿using Marvin.IDP.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marvin.IDP.DbContexts
{
    public class IdentityDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserClaim> UserClaims { get; set; }         

        public IdentityDbContext(
          DbContextOptions<IdentityDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .HasIndex(u => u.Subject)
            .IsUnique();

            modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                    Password = "password",
                    Subject = "d860efca-22d9-47fd-8249-791ba61b07c7",
                    UserName = "David",
                    Active = true,
                    ConcurrencyStamp = "4f31cb3e-ed9c-4a52-9cf6-9f2a4e1b1a01"
                },
                new User()
                {
                    Id = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                    Password = "password",
                    Subject = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                    UserName = "Emma",
                    Active = true,
                    ConcurrencyStamp = "a7a9e9fd-1f36-4a19-9b4e-0d30f8d7c502"
                });

            modelBuilder.Entity<UserClaim>().HasData(
             new UserClaim()
             {
                 Id = new Guid("1a9e5ad6-f0a0-4b1a-9c3e-2f6b1d9c0101"),
                 UserId = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                 Type = "given_name",
                 Value = "David",
                 ConcurrencyStamp = "c1b5a0d2-7e34-4c89-9a51-3b0e7f2d0101"
             },
             new UserClaim()
             {
                 Id = new Guid("2b8f6be7-01b1-4c2b-8d4f-3a7c2eab0202"),
                 UserId = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                 Type = "family_name",
                 Value = "Flagg",
                 ConcurrencyStamp = "d2c6b1e3-8f45-4d9a-8b62-4c1f8a3e0202"
             },
             new UserClaim()
             {
                 Id = new Guid("3c907cf8-12c2-4d3c-9e5a-4b8d3fbc0303"),
                 UserId = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                 Type = "country",
                 Value = "nl",
                 ConcurrencyStamp = "e3d7c2f4-9a56-4eab-9c73-5d2a9b4f0303"
             },
             new UserClaim()
             {
                 Id = new Guid("4da18d09-23d3-4e4d-af6b-5c9e40cd0404"),
                 UserId = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                 Type = "role",
                 Value = "FreeUser",
                 ConcurrencyStamp = "f4e8d3a5-ab67-4fbc-8d84-6e3b0c5a0404"
             },
             new UserClaim()
             {
                 Id = new Guid("5eb29e1a-34e4-4f5e-b07c-6daf51de0505"),
                 UserId = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                 Type = "given_name",
                 Value = "Emma",
                 ConcurrencyStamp = "a5f9e4b6-bc78-40cd-9e95-7f4c1d6b0505"
             },
             new UserClaim()
             {
                 Id = new Guid("6fc3af2b-45f5-406f-818d-7eb062ef0606"),
                 UserId = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                 Type = "family_name",
                 Value = "Flagg",
                 ConcurrencyStamp = "b60af5c7-cd89-41de-8fa6-8a5d2e7c0606"
             },
             new UserClaim()
             {
                 Id = new Guid("70d4b03c-5606-4170-929e-8fc173f00707"),
                 UserId = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                 Type = "country",
                 Value = "be",
                 ConcurrencyStamp = "c71ba6d8-de9a-42ef-90b7-9b6e3f8d0707"
             },
             new UserClaim()
             {
                 Id = new Guid("81e5c14d-6717-4281-a3af-90d284010808"),
                 UserId = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                 Type = "role",
                 Value = "PayingUser",
                 ConcurrencyStamp = "d82cb7e9-efab-4300-81c8-ac7f409e0808"
             });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // get updated entries
            var updatedConcurrencyAwareEntries = ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Modified)
                    .OfType<IConcurrencyAware>();

            foreach (var entry in updatedConcurrencyAwareEntries)
            {
                entry.ConcurrencyStamp = Guid.NewGuid().ToString();
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
