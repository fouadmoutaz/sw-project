using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using onlinepharmacy.Models;

namespace onlinepharmacy.Data
{
    public class onlinepharmacyContext : DbContext
    {
        public onlinepharmacyContext (DbContextOptions<onlinepharmacyContext> options)
            : base(options)
        {
        }

        public DbSet<onlinepharmacy.Models.product>? product { get; set; }

        public DbSet<onlinepharmacy.Models.usersaccounts>? usersaccounts { get; set; }

        public DbSet<onlinepharmacy.Models.orders>? orders { get; set; }

        public DbSet<onlinepharmacy.Models.report>? report { get; set; }
    }
}
