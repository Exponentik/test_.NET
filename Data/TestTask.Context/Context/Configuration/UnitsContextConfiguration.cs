using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Context.Entities;

namespace TestTask.Context
{
    public static class UnitsContextConfiguration
    {
        public static void ConfigureLikes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Unit>().ToTable("units");
        }

    }

}
