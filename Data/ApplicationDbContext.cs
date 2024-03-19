using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MPA.Models;

namespace MPA.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Alloy> Alloys { get; set; }
        public DbSet<AlloyOrder> AlloyOrders { get; set; }
        public DbSet<RCPMonthly> RCPMontlies { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ReceivedAlloyOrder> ReceivedAlloyOrders { get; set; }
        public DbSet<ReceivedOrder> ReceivedOrders { get; set; }
        public DbSet<SupplierEmail> SupplierEmails { get; set; }
    }
}
