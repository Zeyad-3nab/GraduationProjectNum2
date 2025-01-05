using GraduationProject.API.DAL.Models.ApartmentModels;
using GraduationProject.API.DAL.Models.IdentityModels;
using GraduationProject.API.DAL.Models.Sanaie;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.API.DAL.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext) : base(dbContext)
        {

        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<ApartmentImages> ApartmentImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Department to ApplicationUser relationship
            modelBuilder.Entity<Department>()
                .HasMany(d => d.ApplicationUsers)
                .WithOne(u => u.Department)
                .HasForeignKey(u => u.DepartmentId);

            // ApplicationUser to Apartment relationship
            //modelBuilder.Entity<ApplicationUser>()
            //    .HasMany(u => u.Apartments)
            //    .WithOne(a => a.User)
            //    .HasForeignKey(a => a.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Apartment to ApartmentImages relationship
            modelBuilder.Entity<Apartment>()
                .HasMany(a => a.ApartmentImages)
                .WithOne(ai => ai.Apartment)
                .HasForeignKey(ai => ai.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
