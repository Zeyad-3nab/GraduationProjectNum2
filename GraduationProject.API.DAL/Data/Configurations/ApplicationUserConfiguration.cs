using GraduationProject.API.DAL.Models.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.API.DAL.Data.Configurations
{
    public class ApplicationUserConfiguration :IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasIndex(u => u.NationalId).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique(true);
        }
    }
}
