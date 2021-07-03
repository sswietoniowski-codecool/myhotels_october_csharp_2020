using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyHotels.WebApi.Domain;

namespace MyHotels.WebApi.Configurations.Entities
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
                new Country { Id = 1, Name = "Poland", Code = "PL" },
                new Country { Id = 2, Name = "Germany", Code = "DE" },
                new Country { Id = 3, Name = "United States", Code = "US" }
            );
        }
    }
}
