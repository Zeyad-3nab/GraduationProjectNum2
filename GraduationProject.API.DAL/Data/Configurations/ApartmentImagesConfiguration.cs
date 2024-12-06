using GraduationProject.API.DAL.Models.ApartmentModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.API.DAL.Data.Configurations
{
    public class ApartmentImagesConfiguration : IEntityTypeConfiguration<ApartmentImages>
    {
        public void Configure(EntityTypeBuilder<ApartmentImages> builder)
        {
            builder.HasOne(e => e.Apartment)
               .WithMany()
               .HasForeignKey(e => e.ApartmentId);
        }
    }
}
