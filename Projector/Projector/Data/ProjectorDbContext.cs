﻿using Microsoft.EntityFrameworkCore;

namespace Projector.Data
{
    public class ProjectorDbContext : DbContext
    {
        /* CONSTRUCTORS */
        public ProjectorDbContext() { }

        public ProjectorDbContext(DbContextOptions<ProjectorDbContext> options) : base(options) {
        
        }

        /* PROPERTIES */
        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<VerificationLink> VerificationLinks { get; set; }

        /* METHODS */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                    //optionsBuilder.UseSqlServer(@"Server=.\SQLExpress;Database=Projector;Trusted_Connection=True;TrustServerCertificate=True;");
                    optionsBuilder.UseSqlServer(@"Data Source =.\MSSQL2022; Initial Catalog = Projector; Integrated Security = True;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(p => p.Person)
                .WithOne(u => u.User)
                .HasForeignKey<Person>(p => p.UserId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.IsVerified)
                .HasConversion<int>();

            modelBuilder.Entity<Person>()
                .Property(p => p.IsDeleted)
                .HasConversion<int>();

            modelBuilder.Entity<Project>()
                .Property(p => p.IsDeleted)
                .HasConversion<int>();

            modelBuilder.Entity<VerificationLink>()
                .HasOne(v => v.User)
                .WithOne(u => u.VerificationLink)
                .HasForeignKey<VerificationLink>(u => u.Id)
                .IsRequired();
        }
    }
}
