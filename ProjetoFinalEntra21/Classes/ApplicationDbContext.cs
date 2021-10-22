using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoFinalEntra21.Models;
using ProjetoFinalEntra21.Models.Classes_Models;
namespace ProjetoFinalEntra21.Data
{
    public class ApplicationDbContext : IdentityDbContext<User,IdentityRole,string>     
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {
           
        }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Plate> Plate { get; set; }
    }
}
